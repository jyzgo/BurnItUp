//using System.Diagnostics;

using UnityEngine;

namespace MTUnity.Actions
{
    public class MTReverseTime : MTFiniteTimeAction
    {
        public MTFiniteTimeAction Other { get; private set; }


        #region Constructors

        public MTReverseTime (MTFiniteTimeAction action) : base (action.Duration)
        {
            Other = action;
        }

        #endregion Constructors


        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTReverseTimeState (this, target);

        }

        public override MTFiniteTimeAction Reverse ()
        {
            return Other;
        }
    }

    public class MTReverseTimeState : MTFiniteTimeActionState
    {

        protected MTFiniteTimeAction Other { get; set; }

        protected MTFiniteTimeActionState OtherState { get; set; }

        public MTReverseTimeState (MTReverseTime action, GameObject target)
            : base (action, target)
        {   
            Other = action.Other;
            OtherState = (MTFiniteTimeActionState)Other.StartAction (target);
        }

        protected internal override void Stop ()
        {
            OtherState.Stop ();
        }

        public override void Update (float time)
        {
            if (Other != null)
            {
                OtherState.Update (1 - time);
            }
        }

    }

}