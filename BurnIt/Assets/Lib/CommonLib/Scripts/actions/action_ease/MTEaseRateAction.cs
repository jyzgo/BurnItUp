using UnityEngine;

namespace MTUnity.Actions
{
    public class MTEaseRateAction : MTActionEase
    {
        public float Rate { get; private set; }


        #region Constructors

        public MTEaseRateAction (MTFiniteTimeAction action, float rate) : base (action)
        {
            Rate = rate;
        }

        #endregion Constructors


        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTEaseRateActionState (this, target);
        }

        public override MTFiniteTimeAction Reverse ()
        {
            return new MTEaseRateAction ((MTFiniteTimeAction)InnerAction.Reverse (), 1 / Rate);
        }
    }


    #region Action state

    public class MTEaseRateActionState : MTActionEaseState
    {
        protected float Rate { get; private set; }

        public MTEaseRateActionState (MTEaseRateAction action, GameObject target) : base (action, target)
        {
            Rate = action.Rate;
        }

        public override void Update (float time)
        {
            InnerActionState.Update (MTEaseMath.ExpoOut (time));
        }
    }

    #endregion Action state
}