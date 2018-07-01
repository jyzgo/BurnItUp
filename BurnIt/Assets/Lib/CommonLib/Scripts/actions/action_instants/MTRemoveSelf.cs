using System;
using System.Collections.Generic;

using UnityEngine;

namespace MTUnity.Actions
{
    public class MTRemoveSelf : MTActionInstant
    {
        public bool IsNeedCleanUp { get; private set; }

        #region Constructors

        public MTRemoveSelf ()
            : this (true)
        {
        }

        public MTRemoveSelf (bool isNeedCleanUp)
        {
            IsNeedCleanUp = isNeedCleanUp;
        }

        #endregion Constructors


        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTRemoveSelfState (this, target);

        }


        public override MTFiniteTimeAction Reverse ()
        {
            return new MTRemoveSelf (IsNeedCleanUp);
        }
    }

    public class MTRemoveSelfState : MTActionInstantState
    {
        protected bool IsNeedCleanUp { get; set; }

        public MTRemoveSelfState (MTRemoveSelf action, GameObject target)
            : base (action, target)
        {   
            IsNeedCleanUp = action.IsNeedCleanUp;
        }

        public override void Update (float time)
        {
            if (Target && Target.gameObject) 
            {
                UnityEngine.Object.Destroy(Target.gameObject);
            }
        }

    }

}