using System;

using UnityEngine;

namespace MTUnity.Actions
{
    public class MTEaseIn : MTEaseRateAction
    {
        #region Constructors

        public MTEaseIn (MTFiniteTimeAction action, float rate) : base (action, rate)
        {
        }

        #endregion Constructors


        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTEaseInState (this, target);
        }

        public override MTFiniteTimeAction Reverse ()
        {
            return new MTEaseIn ((MTFiniteTimeAction)InnerAction.Reverse (), 1 / Rate);
        }
    }


    #region Action state

    public class MTEaseInState : MTEaseRateActionState
    {
        public MTEaseInState (MTEaseIn action, GameObject target) : base (action, target)
        {
        }

        public override void Update (float time)
        {
            InnerActionState.Update ((float)Math.Pow (time, Rate));
        }
    }

    #endregion Action state

}