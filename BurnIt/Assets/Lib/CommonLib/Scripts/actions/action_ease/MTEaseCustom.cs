using System;

using UnityEngine;

namespace MTUnity.Actions
{
    public partial class MTEaseCustom : MTActionEase
    {
        public Func<float, float> EaseFunc { get; private set; }


        #region Constructors

        public MTEaseCustom (MTFiniteTimeAction action, Func<float, float> easeFunc) : base (action)
        {
            EaseFunc = easeFunc;
        }

        #endregion Constructors


        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTEaseCustomState (this, target);
        }

        public override MTFiniteTimeAction Reverse ()
        {
            return new MTReverseTime (this);
        }
    }


    #region Action state

    public class MTEaseCustomState : MTActionEaseState
    {
        protected Func<float, float> EaseFunc { get; private set; }

        public MTEaseCustomState (MTEaseCustom action, GameObject target) : base (action, target)
        {
            EaseFunc = action.EaseFunc;
        }

        public override void Update (float time)
        {
            InnerActionState.Update (EaseFunc (time));
        }
    }

    #endregion Action state
}