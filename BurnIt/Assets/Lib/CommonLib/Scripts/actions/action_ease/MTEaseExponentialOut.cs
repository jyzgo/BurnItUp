using System;

using UnityEngine;

namespace MTUnity.Actions
{
    public class MTEaseExponentialOut : MTActionEase
    {
        #region Constructors

        public MTEaseExponentialOut (MTFiniteTimeAction action) : base (action)
        {
        }

        #endregion Constructors


        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTEaseExponentialOutState (this, target);
        }

        public override MTFiniteTimeAction Reverse ()
        {
            return new MTEaseExponentialIn ((MTFiniteTimeAction)InnerAction.Reverse ());
        }
    }


    #region Action state

    public class MTEaseExponentialOutState : MTActionEaseState
    {
        public MTEaseExponentialOutState (MTEaseExponentialOut action, GameObject target) : base (action, target)
        {
        }

        public override void Update (float time)
        {
            InnerActionState.Update (MTEaseMath.ExpoOut (time));
        }
    }

    #endregion Action state
}