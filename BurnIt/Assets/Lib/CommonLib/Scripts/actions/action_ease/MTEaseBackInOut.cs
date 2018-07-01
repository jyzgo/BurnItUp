using UnityEngine;

namespace MTUnity.Actions
{
    public class MTEaseBackInOut : MTActionEase
    {
        #region Constructors

        public MTEaseBackInOut(MTFiniteTimeAction action) : base (action)
        {
        }

        #endregion Constructors


        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTEaseBackInOutState (this, target);
        }

        public override MTFiniteTimeAction Reverse()
        {
            return new MTEaseBackInOut ((MTFiniteTimeAction)InnerAction.Reverse ());
        }
    }


    #region Action state

    public class MTEaseBackInOutState : MTActionEaseState
    {
        public MTEaseBackInOutState (MTEaseBackInOut action, GameObject target) : base (action, target)
        {
        }

        public override void Update (float time)
        {
            InnerActionState.Update (MTEaseMath.BackInOut (time));
        }
    }

    #endregion Action state
}