using System;
//using Microsoft.Xna.Framework;



using UnityEngine;

namespace MTUnity.Actions
{
    public class MTEaseElasticIn : MTEaseElastic
    {
        #region Constructors

        public MTEaseElasticIn (MTFiniteTimeAction action) : this (action, 0.3f)
        {
        }

        public MTEaseElasticIn (MTFiniteTimeAction action, float period) : base (action, period)
        {
        }

        #endregion Constructors


        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTEaseElasticInState (this, target);
        }

        public override MTFiniteTimeAction Reverse ()
        {
            return new MTEaseElasticOut ((MTFiniteTimeAction)InnerAction.Reverse (), Period);
        }
    }


    #region Action state

    public class MTEaseElasticInState : MTEaseElasticState
    {
        public MTEaseElasticInState (MTEaseElasticIn action, GameObject target) : base (action, target)
        {
        }

        public override void Update (float time)
        {
            InnerActionState.Update (MTEaseMath.ElasticIn (time, Period));
        }
    }

    #endregion Action state
}