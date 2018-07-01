using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MTUnity.Actions;
using System.Collections.Generic;

public class ScrollHelper {
	ScrollRect _scrollRect;
	public ScrollRect ScrollRect { get { return _scrollRect; } }

	RectTransform _contentHolder;
	public RectTransform contentHolder { get { return _contentHolder; } }

	GameObject _content;
	public GameObject content { get { return _content; } }

	Rect _elasticRect;

	float minHPos = 0;
	float maxHPos = 1;

	float minVPos = 0;
	float maxVPos = 1;

    float minScrollX;
    float maxScrollX;
	float minScrollY;
	float maxScrollY;

	bool _autoScrollActionInterruptable = false;
	List<MTMoveTo> _autoScrollActions = new List<MTMoveTo>();
	MTSequenceState _autoScrollCurrState = null;

	public ScrollHelper (ScrollRect scrollRect, RectTransform contentHolder) {
		_contentHolder = contentHolder;
		_scrollRect = scrollRect;
		_scrollRect.onValueChanged.AddListener (OnScrolling);
	}

	public void SetContent (GameObject content, bool destroyOld) {
		if (_content != content) {
			if (_content != null) {
				if (destroyOld) {
					Object.Destroy (_content);
				} else {
					_content.transform.SetParent (null, false);
				}
			}

			_content = content;
			if (_content != null) {
				_content.transform.SetParent (_contentHolder, false);
			}
		}

		UpdateContentPos ();
	}

	public void AutoScrollAction(bool interruptable, params MTMoveTo[] moveActions) {
		StopAutoScrollAction ();
		_autoScrollActionInterruptable = interruptable;
		for(int i = 0; i < moveActions.Length; i++) {
			_autoScrollActions.Add (moveActions [i]);
		}
		StartNextScrollAction ();
	}

	public void StopAutoScrollAction() {
		_autoScrollActions.Clear ();
		_contentHolder.gameObject.StopAction (_autoScrollCurrState);
		_autoScrollCurrState = null;
	}

	void StartNextScrollAction() {
		if (_autoScrollCurrState != null) {
			_autoScrollActions.Remove (_autoScrollCurrState.Actions(0) as MTMoveTo);
			_autoScrollCurrState = null;
		}

		if (_autoScrollActions.Count == 0) {
			return;
		}
			
		MTSequence action = new MTSequence (_autoScrollActions[0], new MTCallFunc(delegate {
			StartNextScrollAction();
		}));
		_autoScrollCurrState = _contentHolder.gameObject.RunAction (action) as MTSequenceState;
	}

	void CheckInterruptAutoScrollAction() {
		if (_autoScrollActions.Count > 0 && _autoScrollActionInterruptable) {
			if (_autoScrollCurrState != null) {
				MTActionState state = _autoScrollCurrState.ActionStates (0);
				if (state == null || ((state as MTMoveToState).PreviousPosition) != ((state as MTMoveToState).IsWorld ? _contentHolder.position : _contentHolder.localPosition)) {
					StopAutoScrollAction ();
				}
			}
		}
	}

	public Rect elasticRect {
		get { return _elasticRect; }
		set {
			_elasticRect = value;

			UpdateContentPos ();

			minHPos = 0;
			maxHPos = 1;
			minVPos = 0;
			maxVPos = 1;

			if (_elasticRect.width < 1 || _elasticRect.height < 1) {
				_contentHolder.sizeDelta = new Vector2 (0, 0);
			} else {
				_contentHolder.sizeDelta = new Vector2 (_elasticRect.width, _elasticRect.height);

				if (_content != null) {
					Vector2 scrollerSize = (_scrollRect.transform as RectTransform).rect.size;
					Vector2 contentSize = (_content.transform as RectTransform).sizeDelta;
					if (_scrollRect.horizontal) {
                        float leftOffset = _elasticRect.xMin / (_elasticRect.width - scrollerSize.x);
						if (leftOffset > 0) {
							minHPos -= leftOffset;
						}
                        float rightOffset = (contentSize.x - _elasticRect.xMax) / (_elasticRect.width - scrollerSize.x);
						if (rightOffset > 0) {
							maxHPos += rightOffset;
						}
					}
					if (_scrollRect.vertical) {
                        float bottomOffset = (contentSize.y - _elasticRect.yMax) / (_elasticRect.height - scrollerSize.y);
						if (bottomOffset > 0) {
							minVPos -= bottomOffset;
						}
                        float topOffset = _elasticRect.yMin / (_elasticRect.height - scrollerSize.y);
						if (topOffset > 0) {
							maxVPos += topOffset;
						}
					}
                    UpdateScrollX(scrollerSize);
					UpdateScrollY (scrollerSize);
				}
			}
		}
	}

    void UpdateScrollX(Vector2 scrollerSize){
        //计算不被弹的情况下最高Y值
        maxScrollX = (_elasticRect.width - scrollerSize.x) / 2;
        //按图比例缩小
        maxScrollX *= 0.01f;

        minScrollX = -maxScrollX;
    }
	void UpdateScrollY(Vector2 scrollerSize){
		//计算不被弹的情况下最高Y值
		maxScrollY = (_elasticRect.height - scrollerSize.y) / 2;
		//按图比例缩小
		maxScrollY *= 0.01f;

		minScrollY = -maxScrollY;
	}

	public Vector3 GetCorrectedContentPos(Vector3 pos){

        float x = Mathf.Clamp (pos.x, minScrollX, maxScrollX);
		float y = Mathf.Clamp (pos.y, minScrollY, maxScrollY);

		return new Vector3 (x, y, pos.z);
	}

	void UpdateContentPos () {
		if (_content == null) return;

		RectTransform rt = _content.transform as RectTransform;
		Vector3 pos = rt.localPosition;
		if (_elasticRect.width < 1 || _elasticRect.height < 1) {
			pos.x = 0;
			pos.y = 0;
		} else {
			Vector2 size = rt.sizeDelta;
			pos.x = (size.x - _elasticRect.xMax - _elasticRect.xMin) * 0.5f;
			pos.y = (_elasticRect.yMin - (size.y - _elasticRect.yMax)) * 0.5f;
		}
		rt.localPosition = pos;

	}

	public void SetActive(bool value){
		_scrollRect.enabled = value;
	}

	void OnScrolling (Vector2 position) {
		CheckInterruptAutoScrollAction ();
		if (_scrollRect.horizontal) {
			if (_scrollRect.horizontalNormalizedPosition < minHPos) {
				_scrollRect.horizontalNormalizedPosition = minHPos;
			} else if (_scrollRect.horizontalNormalizedPosition > maxHPos) {
				_scrollRect.horizontalNormalizedPosition = maxHPos;
			}
		}
		if (_scrollRect.vertical) {
			if (_scrollRect.verticalNormalizedPosition < minVPos) {
				_scrollRect.verticalNormalizedPosition = minVPos;
			} else if (_scrollRect.verticalNormalizedPosition > maxVPos) {
				_scrollRect.verticalNormalizedPosition = maxVPos;
			}
		}
	}

}
