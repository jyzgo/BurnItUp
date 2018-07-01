using System;

using UnityEngine;

namespace MTUnity.Actions
{
    public class MTEaseExponentialIn : MTActionEase
    {
        #region Constructors

        public MTEaseExponentialIn (MTFiniteTimeAction action) : base (action)
        {
        }

        #endregion Constructors


        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTEaseExponentialInState (this, target);
        }

        public override MTFiniteTimeAction Reverse ()
        {
            return new MTEaseExponentialOut ((MTFiniteTimeAction)InnerAction.Reverse ());
        }
    }


    #region Action state

    public class MTEaseExponentialInState : MTActionEaseState
    {
        public MTEaseExponentialInState (MTEaseExponentialIn action, GameObject target) : base (action, target)
        {
        }

        public override void Update (float time)
        {
            InnerActionState.Update (MTEaseMath.ExpoIn (time));
        }
    }

    #endregion Action state
}