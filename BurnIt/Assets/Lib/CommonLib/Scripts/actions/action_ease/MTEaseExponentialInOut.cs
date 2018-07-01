using System;

using UnityEngine;

namespace MTUnity.Actions
{
    public class MTEaseExponentialInOut : MTActionEase
    {
        #region Constructors

        public MTEaseExponentialInOut (MTFiniteTimeAction action) : base(action)
        {
        }

        #endregion Constructors


        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTEaseExponentialInOutState (this, target);
        }

        public override MTFiniteTimeAction Reverse ()
        {
            return new MTEaseExponentialInOut ((MTFiniteTimeAction)InnerAction.Reverse ());
        }
    }


    #region Action state

    public class MTEaseExponentialInOutState : MTActionEaseState
    {
        public MTEaseExponentialInOutState (MTEaseExponentialInOut action, GameObject target) : base (action, target)
        {
        }

        public override void Update (float time)
        {
            InnerActionState.Update (MTEaseMath.ExpoInOut (time));
        }
    }

    #endregion Action state
}