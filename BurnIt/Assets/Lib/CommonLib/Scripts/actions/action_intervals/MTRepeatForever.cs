//using System.Diagnostics;

using UnityEngine;

namespace MTUnity.Actions
{
    public class MTRepeatForever : MTFiniteTimeAction
    {
        public MTFiniteTimeAction InnerAction { get; private set; }


        #region Constructors

        public MTRepeatForever (params MTFiniteTimeAction[] actions)
        {
            Debug.Assert (actions != null);
            InnerAction = new MTSequence (actions);

        }

        public MTRepeatForever (MTFiniteTimeAction action)
        {
            Debug.Assert (action != null);
            InnerAction = action;
        }

        #endregion Constructors

        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTRepeatForeverState (this, target);

        }

        public override MTFiniteTimeAction Reverse ()
        {
            return new MTRepeatForever (InnerAction.Reverse () as MTFiniteTimeAction);
        }
    }

    public class MTRepeatForeverState : MTFiniteTimeActionState
    {

        private MTFiniteTimeAction InnerAction { get; set; }

        private MTFiniteTimeActionState InnerActionState { get; set; }

        public MTRepeatForeverState (MTRepeatForever action, GameObject target)
            : base (action, target)
        { 
            InnerAction = action.InnerAction;
            InnerActionState = (MTFiniteTimeActionState)InnerAction.StartAction (target);
        }

        protected internal override void Step (float dt)
        {
            InnerActionState.Step (dt);

            if (InnerActionState.IsDone)
            {
                float diff = InnerActionState.Elapsed - InnerActionState.Duration;
                InnerActionState = (MTFiniteTimeActionState)InnerAction.StartAction (Target);
                InnerActionState.Step (0f);
                InnerActionState.Step (diff);
            }
        }

        public override bool IsDone {
            get { return false; }
        }

    }
}