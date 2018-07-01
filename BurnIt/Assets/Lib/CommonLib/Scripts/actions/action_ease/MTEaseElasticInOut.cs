using System;
//using Microsoft.Xna.Framework;



using UnityEngine;

namespace MTUnity.Actions
{
    public class MTEaseElasticInOut : MTEaseElastic
    {
        #region Constructors

        public MTEaseElasticInOut (MTFiniteTimeAction action) : this (action, 0.3f)
        {
        }

        public MTEaseElasticInOut (MTFiniteTimeAction action, float period) : base (action, period)
        {
        }

        #endregion Constructors


        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTEaseElasticInOutState (this, target);
        }

        public override MTFiniteTimeAction Reverse ()
        {
            return new MTEaseElasticInOut ((MTFiniteTimeAction)InnerAction.Reverse (), Period);
        }
    }


    #region Action state

    public class MTEaseElasticInOutState : MTEaseElasticState
    {
        public MTEaseElasticInOutState (MTEaseElasticInOut action, GameObject target) : base (action, target)
        {
        }

        public override void Update (float time)
        {
            InnerActionState.Update (MTEaseMath.ElasticInOut (time, Period));
        }
    }

    #endregion Action state
}