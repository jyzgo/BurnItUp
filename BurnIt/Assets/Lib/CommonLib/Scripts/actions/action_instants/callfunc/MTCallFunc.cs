using System;

using UnityEngine;

namespace MTUnity.Actions
{
    public class MTCallFunc : MTActionInstant
    {
        public Action CallFunction { get; private set;}
        public string ScriptFuncName { get; private set; }


        #region Constructors

        public MTCallFunc()
        {
            ScriptFuncName = "";
            CallFunction = null;
        }

        public MTCallFunc(Action selector) : base()
        {
            CallFunction = selector;
        }

        #endregion Constructors

        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTCallFuncState (this, target);

        }

    }

    public class MTCallFuncState : MTActionInstantState
    {

        protected Action CallFunction { get; set;}
        protected string ScriptFuncName { get; set; }

        public MTCallFuncState (MTCallFunc action, GameObject target)
            : base(action, target)
        {   
            CallFunction = action.CallFunction;
            ScriptFuncName = action.ScriptFuncName;
        }

        public virtual void Execute()
        {
            if (null != CallFunction)
            {
                CallFunction();
				CallFunction = null;
            }
        }

        public override void Update (float time)
        {
            Execute();
        }
    }
}