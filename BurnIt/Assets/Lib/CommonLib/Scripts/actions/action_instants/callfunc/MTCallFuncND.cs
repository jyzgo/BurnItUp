using System;

using UnityEngine;

namespace MTUnity.Actions
{
    public class MTCallFuncND : MTCallFuncN
    {
        public Action<GameObject, object> CallFunctionND { get; private set; }
        public object Data { get; private set; }


        #region Constructors

        public MTCallFuncND(Action<GameObject, object> selector, object d) : base()
        {
            Data = d;
            CallFunctionND = selector;
        }

        #endregion Constructors


        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTCallFuncNDState (this, target);

        }
    }

    public class MTCallFuncNDState : MTCallFuncState
    {
        protected Action<GameObject, object> CallFunctionND { get; set; }
        protected object Data { get; set; }

        public MTCallFuncNDState (MTCallFuncND action, GameObject target)
            : base(action, target)
        {   
            CallFunctionND = action.CallFunctionND;
            Data = action.Data;
        }

        public override void Execute()
        {
            if (null != CallFunctionND)
            {
                CallFunctionND(Target, Data);
				CallFunctionND = null;
            }
        }
    }
}