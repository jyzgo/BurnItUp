using UnityEngine;

namespace MTUnity.Actions
{
    public class MTEaseBounceIn : MTActionEase
    {
        #region Constructors

        public MTEaseBounceIn (MTFiniteTimeAction action) : base (action)
        {
        }

        #endregion Constructors


        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTEaseBounceInState (this, target);
        }

        public override MTFiniteTimeAction Reverse ()
        {
            return new MTEaseBounceOut ((MTFiniteTimeAction)InnerAction.Reverse ());
        }
    }


    #region Action state

    public class MTEaseBounceInState : MTActionEaseState
    {
        public MTEaseBounceInState (MTEaseBounceIn action, GameObject target) : base (action, target)
        {
        }

        public override void Update (float time)
        {
            InnerActionState.Update (MTEaseMath.BounceIn (time));
        }
    }

    #endregion Action state
}