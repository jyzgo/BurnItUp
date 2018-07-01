using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using MonsterLove.StateMachine;
using MTUnity.Actions;

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
    void Playing_Update()
    {
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

