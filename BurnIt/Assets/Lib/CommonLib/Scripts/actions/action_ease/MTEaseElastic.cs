using UnityEngine;

namespace MTUnity.Actions
{
    public class MTEaseElastic : MTActionEase
    {
        public float Period { get; private set; }


        #region Constructors

        public MTEaseElastic (MTFiniteTimeAction action, float period) : base (action)
        {
            Period = period;
        }

        public MTEaseElastic (MTFiniteTimeAction action) : this (action, 0.3f)
        {
        }

        #endregion Constructors


        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTEaseElasticState (this, target);
        }

        public override MTFiniteTimeAction Reverse ()
        {
            return null;
        }
    }


    #region Action state

    public class MTEaseElasticState : MTActionEaseState
    {
        protected float Period { get; private set; }

        public MTEaseElasticState (MTEaseElastic action, GameObject target) : base (action, target)
        {
            Period = action.Period;
        }
    }

    #endregion Action state
}