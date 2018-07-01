using UnityEngine;

namespace MTUnity.Actions
{
    public class MTTargetedAction : MTFiniteTimeAction
    {
        public MTFiniteTimeAction TargetedAction { get; private set; }
        public GameObject ForcedTarget { get; private set; }


        #region Constructors

        public MTTargetedAction (GameObject target, MTFiniteTimeAction action) : base (action.Duration)
        {
            ForcedTarget = target;
            TargetedAction = action;
        }

        #endregion Constructors


        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTTargetedActionState (this, target);
        }

        public override MTFiniteTimeAction Reverse()
        {
            return new MTTargetedAction (ForcedTarget, TargetedAction.Reverse ());
        }
    }

    public class MTTargetedActionState : MTFiniteTimeActionState
    {
        protected MTFiniteTimeAction TargetedAction { get; set; }

        protected MTFiniteTimeActionState ActionState { get; set; }

        protected GameObject ForcedTarget { get; set; }

        public MTTargetedActionState (MTTargetedAction action, GameObject target)
            : base (action, target)
        {   
            ForcedTarget = action.ForcedTarget;
            TargetedAction = action.TargetedAction;

            ActionState = (MTFiniteTimeActionState)TargetedAction.StartAction (ForcedTarget);
        }

        protected internal override void Stop ()
        {
            ActionState.Stop ();
        }

        public override void Update (float time)
        {
            ActionState.Update (time);
        }


    }

}