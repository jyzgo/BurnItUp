using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using MTUnity.Utils;

namespace MTUnity.UI
{
	public class ModalOverlay : MonoBehaviour
	{
		Image _view;
		bool _isNewView;
		BoxCollider2D _collider;
		bool _isNewCollider;

		public Color color;
		public bool addCollider = true;
		public Action clickHandler;

		public new bool enabled {
			get { return base.enabled; }
			set {
				base.enabled = value;
				if (_view != null) _view.enabled = value;
				if (_collider != null) _collider.enabled = value;
			}
		}

		void Start ()
		{
			CreateView ();
			if (addCollider) CreateCollider ();
			enabled = enabled;
		}

		void CreateView ()
		{
			_view = gameObject.GetComponent<Image> ();
			Color defalutColor = new Color (1, 1, 1, 1);
			if (_view == null) {
				_view = gameObject.AddComponent<Image> ();
				_isNewView = true;
			} else {
				defalutColor = _view.color;
			}
			Texture2D tex = AssetManager.GetSmallTexture (color);
			_view.sprite = Sprite.Create (tex, new Rect (0, 0, tex.width, tex.height), new Vector2 (0.5f, 0.5f));
			_view.material = AssetManager.spritesDefaultMaterial;
			_view.color = defalutColor;
			RectTransform rt = _view.rectTransform;
			rt.anchorMin = new Vector2 (0, 0);
			rt.anchorMax = new Vector2 (1, 1);
			rt.offsetMin = new Vector2 (0, 0);
			rt.offsetMax = new Vector2 (0, 0);

			EventTriggerListener.Get (gameObject).onClick += OnClick;
		}

		void CreateCollider ()
		{
			Rect rect = (transform as RectTransform).rect;
			Vector3 pos = transform.localPosition;
			_collider = gameObject.GetComponent<BoxCollider2D> ();
			if (_collider == null) {
				_collider = gameObject.AddComponent<BoxCollider2D> ();
				_isNewCollider = true;
			}
			_collider.size = new Vector2 (rect.width, rect.height);
			_collider.offset = new Vector2 (-pos.x, -pos.y);
		}

		void OnClick (GameObject go)
		{
			if (clickHandler != null) clickHandler ();
		}

		void OnDestroy ()
		{
			if (_view != null && _isNewView) GameObject.Destroy (_view);
			if (_collider != null && _isNewCollider) GameObject.Destroy (_collider);
		}

	}

}