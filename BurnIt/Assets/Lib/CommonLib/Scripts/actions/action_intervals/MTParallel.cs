using System;
using System.Collections.Generic;

using UnityEngine;

namespace MTUnity.Actions
{
    public class MTParallel : MTFiniteTimeAction
    {
        public MTFiniteTimeAction[] Actions { get; private set; }

        #region Constructors

        public MTParallel (params MTFiniteTimeAction[] actions) : base ()
        {
            // Can't call base(duration) because max action duration needs to be determined here
            float maxDuration = 0.0f;


            for (int i = 0; i < actions.Length; ++i) 
            {
                var action = actions[i];
                if (action.Duration > maxDuration)
                {
                    maxDuration = action.Duration;
                }
            }


            Duration = maxDuration;

            Actions = actions;

            for (int i = 0; i < Actions.Length; i++)
            {
                var actionDuration = Actions [i].Duration;
                if (actionDuration < Duration)
                {
                    Actions [i] = new MTSequence (Actions [i], new MTDelayTime (Duration - actionDuration));
                }
            }
        }

        #endregion Constructors


        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTParallelState (this, target);

        }

        public override MTFiniteTimeAction Reverse ()
        {
            MTFiniteTimeAction[] rev = new MTFiniteTimeAction[Actions.Length];
            for (int i = 0; i < Actions.Length; i++)
            {
                rev [i] = Actions [i].Reverse ();
            }

            return new MTParallel (rev);
        }

    }

    public class MTParallelState : MTFiniteTimeActionState
    {

        protected MTFiniteTimeAction[] Actions { get; set; }

        protected MTFiniteTimeActionState[] ActionStates { get; set; }

        public MTParallelState (MTParallel action, GameObject target)
            : base (action, target)
        {   
            Actions = action.Actions;
            ActionStates = new MTFiniteTimeActionState[Actions.Length];

            for (int i = 0; i < Actions.Length; i++)
            {
                ActionStates [i] = (MTFiniteTimeActionState)Actions [i].StartAction (target);
            }
        }

        protected internal override void Stop ()
        {
            for (int i = 0; i < Actions.Length; i++)
            {
                ActionStates [i].Stop ();
            }
            base.Stop ();
        }

        public override void Update (float time)
        {
            for (int i = 0; i < Actions.Length; i++)
            {
                ActionStates [i].Update (time);
            }
        }
    }
}