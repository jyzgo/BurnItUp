using System;

using UnityEngine;

namespace MTUnity.Actions
{
    public class MTEaseSineOut : MTActionEase
    {
        #region Constructors

        public MTEaseSineOut (MTFiniteTimeAction action) : base (action)
        {
        }

        #endregion Constructors


        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTEaseSineOutState (this, target);
        }

        public override MTFiniteTimeAction Reverse ()
        {
            return new MTEaseSineIn ((MTFiniteTimeAction)InnerAction.Reverse ());
        }
    }


    #region Action state

    public class MTEaseSineOutState : MTActionEaseState
    {
        public MTEaseSineOutState (MTEaseSineOut action, GameObject target) : base (action, target)
        {
        }

        public override void Update (float time)
        {
            InnerActionState.Update (MTEaseMath.SineOut (time));
        }
    }

    #endregion Action state
}