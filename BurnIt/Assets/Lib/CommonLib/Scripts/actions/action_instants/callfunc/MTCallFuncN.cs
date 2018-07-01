using System;

using UnityEngine;

namespace MTUnity.Actions
{
    public class MTCallFuncN : MTCallFunc
    {
        public Action<GameObject> CallFunctionN { get; private set; }

        #region Constructors

        public MTCallFuncN() : base()
        {
        }

        public MTCallFuncN(Action<GameObject> selector) : base()
        {
            CallFunctionN = selector;
        }

        #endregion Constructors


        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTCallFuncNState (this, target);

        }

    }

    public class MTCallFuncNState : MTCallFuncState
    {

        protected Action<GameObject> CallFunctionN { get; set; }

        public MTCallFuncNState (MTCallFuncN action, GameObject target)
            : base(action, target)
        {   
            CallFunctionN = action.CallFunctionN;
        }

        public override void Execute()
        {
            if (null != CallFunctionN)
            {
                CallFunctionN(Target);
				CallFunctionN = null;
            }
            //if (m_nScriptHandler) {
            //    MTScriptEngineManager::sharedManager()->getScriptEngine()->executeFunctionWithobject(m_nScriptHandler, m_pTarget, "GameObject");
            //}
        }

    }
}