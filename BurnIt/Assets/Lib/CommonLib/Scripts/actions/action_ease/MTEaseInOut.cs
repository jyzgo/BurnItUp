using System;

using UnityEngine;

namespace MTUnity.Actions
{
    public class MTEaseInOut : MTEaseRateAction
    {
        #region Constructors

        public MTEaseInOut (MTFiniteTimeAction action, float rate) : base (action, rate)
        {
        }

        #endregion Constructors


        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTEaseInOutState (this, target);
        }

        public override MTFiniteTimeAction Reverse ()
        {
            return new MTEaseInOut ((MTFiniteTimeAction)InnerAction.Reverse (), Rate);
        }
    }


    #region Action state

    public class MTEaseInOutState : MTEaseRateActionState
    {
        public MTEaseInOutState (MTEaseInOut action, GameObject target) : base (action, target)
        {
        }

        public override void Update (float time)
        {
            float actionRate = Rate;
            time *= 2;

            if (time < 1)
            {
                InnerActionState.Update (0.5f * (float)Math.Pow (time, actionRate));
            }
            else
            {
                InnerActionState.Update (1.0f - 0.5f * (float)Math.Pow (2 - time, actionRate));
            }        
        }
    }

    #endregion Action state
}