using UnityEngine;

namespace MTUnity.Actions
{
    public class MTEaseBounceInOut : MTActionEase
    {
        #region Constructors

        public MTEaseBounceInOut (MTFiniteTimeAction action) : base (action)
        {
        }

        #endregion Constructors


        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTEaseBounceInOutState (this, target);
        }

        public override MTFiniteTimeAction Reverse ()
        {
            return new MTEaseBounceInOut ((MTFiniteTimeAction)InnerAction.Reverse ());
        }
    }


    #region Action state

    public class MTEaseBounceInOutState : MTActionEaseState
    {
        public MTEaseBounceInOutState (MTEaseBounceInOut action, GameObject target) : base (action, target)
        {
        }

        public override void Update (float time)
        {
            InnerActionState.Update (MTEaseMath.BounceInOut (time));
        }
    }

    #endregion Action state
}