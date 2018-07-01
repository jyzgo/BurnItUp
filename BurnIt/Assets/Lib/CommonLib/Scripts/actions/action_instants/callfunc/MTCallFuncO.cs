using System;

using UnityEngine;

namespace MTUnity.Actions
{
    public class MTCallFuncO : MTCallFunc
    {
        public Action<object> CallFunctionO { get; private set; }
        public object Object { get; private set; }

        #region Constructors

        public MTCallFuncO()
        {
            Object = null;
            CallFunctionO = null;
        }

        public MTCallFuncO(Action<object> selector, object pObject) : this()
        {
            Object = pObject;
            CallFunctionO = selector;
        }

        #endregion Constructors

        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTCallFuncOState (this, target);

        }

    }

    public class MTCallFuncOState : MTCallFuncState
    {
        protected Action<object> CallFunctionO { get; set; }
        protected object Object { get; set; }

        public MTCallFuncOState (MTCallFuncO action, GameObject target)
            : base(action, target)
        {   
            CallFunctionO = action.CallFunctionO;
            Object = action.Object;
        }

        public override void Execute()
        {
            if (null != CallFunctionO)
            {
                CallFunctionO(Object);
				CallFunctionO = null;
            }
        }
    }
}