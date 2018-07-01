using UnityEngine;

namespace MTUnity.Actions
{
    public class MTEaseBounceOut : MTActionEase
    {
        #region Constructors

        public MTEaseBounceOut (MTFiniteTimeAction action) : base (action)
        {
        }

        #endregion Constructors


        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTEaseBounceOutState (this, target);
        }

        public override MTFiniteTimeAction Reverse ()
        {
            return new MTEaseBounceIn ((MTFiniteTimeAction)InnerAction.Reverse ());
        }
    }


    #region Action state

    public class MTEaseBounceOutState : MTActionEaseState
    {
        public MTEaseBounceOutState (MTEaseBounceOut action, GameObject target) : base (action, target)
        {
        }

        public override void Update (float time)
        {
            InnerActionState.Update (MTEaseMath.BounceOut (time));
        }
    }

    #endregion Action state
}