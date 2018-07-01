using System;

using UnityEngine;

namespace MTUnity.Actions
{
    public class MTEaseOut : MTEaseRateAction
    {
        #region Constructors

        public MTEaseOut (MTFiniteTimeAction action, float rate) : base (action, rate)
        {
        }

        #endregion Constructors


        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTEaseOutState (this, target);
        }

        public override MTFiniteTimeAction Reverse ()
        {
            return new MTEaseOut ((MTFiniteTimeAction)InnerAction.Reverse (), 1 / Rate);
        }
    }


    #region Action state

    public class MTEaseOutState : MTEaseRateActionState
    {
        public MTEaseOutState (MTEaseOut action, GameObject target) : base (action, target)
        {
        }

        public override void Update (float time)
        {
            InnerActionState.Update ((float)(Math.Pow (time, 1 / Rate)));      
        }
    }

    #endregion Action state
}