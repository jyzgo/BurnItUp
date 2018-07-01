using System;
using UnityEngine; 


namespace MTUnity.Actions
{
    public class MTEaseSineInOut : MTActionEase
    {
        #region Constructors

        public MTEaseSineInOut (MTFiniteTimeAction action) : base (action)
        {
        }

        #endregion Constructors


        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTEaseSineInOutState (this, target);
        }

        public override MTFiniteTimeAction Reverse ()
        {
            return new MTEaseSineInOut ((MTFiniteTimeAction)InnerAction.Reverse ());
        }
    }


    #region Action state

    public class MTEaseSineInOutState : MTActionEaseState
    {
        public MTEaseSineInOutState (MTEaseSineInOut action, GameObject target) : base (action, target)
        {
        }

        public override void Update (float time)
        {
            InnerActionState.Update (MTEaseMath.SineInOut (time));
        }
    }

    #endregion Action state
}