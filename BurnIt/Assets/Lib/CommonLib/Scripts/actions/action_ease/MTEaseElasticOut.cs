using System;
//using Microsoft.Xna.Framework;



using UnityEngine;

namespace MTUnity.Actions
{
    public class MTEaseElasticOut : MTEaseElastic
    {
        #region Constructors

        public MTEaseElasticOut (MTFiniteTimeAction action) : base (action, 0.3f)
        {
        }

        public MTEaseElasticOut (MTFiniteTimeAction action, float period) : base (action, period)
        {
        }

        #endregion Constructors


        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTEaseElasticOutState (this, target);
        }

        public override MTFiniteTimeAction Reverse ()
        {
            return new MTEaseElasticIn ((MTFiniteTimeAction)InnerAction.Reverse(), Period);
        }
    }


    #region Action state

    public class MTEaseElasticOutState : MTEaseElasticState
    {
        public MTEaseElasticOutState (MTEaseElasticOut action, GameObject target) : base (action, target)
        {
        }

        public override void Update (float time)
        {
            InnerActionState.Update (MTEaseMath.ElasticOut (time, Period));
        }
    }

    #endregion Action state
}