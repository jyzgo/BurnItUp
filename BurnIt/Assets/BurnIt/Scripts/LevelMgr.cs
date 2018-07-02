using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using MonsterLove.StateMachine;
using MTUnity.Actions;
using Destructible2D;

enum PlayState
{
    Ready,
    Playing,
    Lose
};
public class LevelMgr :MonoBehaviour
{
    const float MOVE_TIME = 0.2f;
    const float JUMP_TIME = 0.2f;

    int _way = 0;

    public static LevelMgr Current;


    StateMachine<PlayState> _fsm;
    UIMgr uiMgr;
    public void Init()
    {
        Physics.gravity = new Vector3(0, -30.0F, 0);
        uiMgr = FindObjectOfType<UIMgr>();

        _fsm = StateMachine<PlayState>.Initialize(this, PlayState.Ready);

    }

    void Awake()
    {
        Current = this;
        Init();
    }


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Ready_Enter()
    {
        Debug.Log("Ready");
        uiMgr.SetStateText("Get Ready!");
        Reset();
        //_fsm.ChangeState(PlayState.Playing);
    }

    void Ready_Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            OnClick(Vector3.zero);
        }
    }
    private void Reset()
    {

        //_player.transform.position = new Vector3(0, 1, 0);
        _way = 0;
    }

    internal void ToLose()
    {
        _fsm.ChangeState(PlayState.Lose);
    }

    IEnumerator Lose_Enter()
    {
        uiMgr.SetStateText("Lose");
        yield return new WaitForSeconds(2f);
        _fsm.ChangeState(PlayState.Ready);
    }

    void Playing_Enter()
    {
        Debug.Log("Playing");
        uiMgr.SetStateText("Playing");
    }

    const float SPEED = 0.05f;
    const int FractureCount = 2;

    public GameObject ExplosionPrefab;
    public float Intercept;
    public float Force = 0f;
    Vector2 explosionPosition;
    void Playing_Update()
    {

        // Required key is down?
        if (Input.GetKeyDown(KeyCode.Mouse0) == true)
        {
            // Main camera exists?
            var mainCamera = Camera.main;

            if (mainCamera != null)
            {
                // Get screen ray of mouse position
                explosionPosition = D2dHelper.ScreenToWorldPosition(Input.mousePosition, Intercept, mainCamera);

                var collider = Physics2D.OverlapPoint(explosionPosition);
               
                if (collider != null)
                {
                    var destructible = collider.GetComponentInParent<D2dDestructible>();

                    if (destructible != null)
                    {
                        // Register split event
                        destructible.OnEndSplit.AddListener(OnEndSplit);

                        // Split via fracture
                        D2dQuadFracturer.Fracture(destructible, FractureCount, 0.5f);

                        // Unregister split event
                        destructible.OnEndSplit.RemoveListener(OnEndSplit);

                        // Spawn explosion prefab?
                        if (ExplosionPrefab != null)
                        {
                            var worldRotation = Quaternion.Euler(0.0f, 0.0f, UnityEngine.Random.Range(0.0f, 360.0f)); // Random rotation around Z axis

                            Instantiate(ExplosionPrefab, explosionPosition, worldRotation);
                        }
                    }
                }
            }
        }
    }
    private void OnEndSplit(List<D2dDestructible> clones)
    {
        // Go through all clones in the clones list
        for (var i = clones.Count - 1; i >= 0; i--)
        {
            var clone = clones[i];
            var rigidbody = clone.GetComponent<Rigidbody2D>();

            // Does this clone have a Rigidbody2D?
            if (rigidbody != null)
            {
                // Get the local point of the explosion that called this split event
                var localPoint = (Vector2)clone.transform.InverseTransformPoint(explosionPosition);

                // Get the vector between this point and the center of the destructible's current rect
                var vector = clone.AlphaRect.center - localPoint;

                // Apply relative force
                rigidbody.AddRelativeForce(vector * Force, ForceMode2D.Impulse);
            }
        }
    }
    public void Touch()
    {
        Debug.Log("Touch");
    }

    
    public void OnClick(Vector3 x)
    {
        if(_fsm.State == PlayState.Ready)
        {
            _fsm.ChangeState(PlayState.Playing);
        }
        else if(_fsm.State == PlayState.Playing)
        {


        }
    }

}

