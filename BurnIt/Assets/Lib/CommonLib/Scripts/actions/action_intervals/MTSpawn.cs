using System;
//using System.Diagnostics;

using UnityEngine;

namespace MTUnity.Actions
{
    public class MTSpawn : MTFiniteTimeAction
    {
        public MTFiniteTimeAction ActionOne { get; protected set; }
        public MTFiniteTimeAction ActionTwo { get; protected set; }


        #region Constructors

        protected MTSpawn (MTFiniteTimeAction action1, MTFiniteTimeAction action2)
            : base (Math.Max (action1.Duration, action2.Duration))
        {
            InitMTSpawn (action1, action2);
        }

        public MTSpawn (params MTFiniteTimeAction[] actions)
        {
            MTFiniteTimeAction prev = actions [0];
            MTFiniteTimeAction next = null;

            if (actions.Length == 1)
            {
                next = new MTExtraAction ();
            }
            else
            {
                // We create a nested set of MTSpawnActions out of all of the actions
                for (int i = 1; i < actions.Length - 1; i++)
                {
                    prev = new MTSpawn (prev, actions [i]);
                }

                next = actions [actions.Length - 1];
            }

            // Can't call base(duration) because we need to determine max duration
            // Instead call base's init method here
            if (prev != null && next != null)
            {
                Duration = Math.Max (prev.Duration, next.Duration);
                InitMTSpawn (prev, next);
            }
        }

        private void InitMTSpawn (MTFiniteTimeAction action1, MTFiniteTimeAction action2)
        {
            Debug.Assert (action1 != null);
            Debug.Assert (action2 != null);

            float d1 = action1.Duration;
            float d2 = action2.Duration;

            ActionOne = action1;
            ActionTwo = action2;

            if (d1 > d2)
            {
                ActionTwo = new MTSequence (action2, new MTDelayTime (d1 - d2));
            }
            else if (d1 < d2)
            {
                ActionOne = new MTSequence (action1, new MTDelayTime (d2 - d1));
            }
        }

        #endregion Constructors


        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTSpawnState (this, target);

        }

        public override MTFiniteTimeAction Reverse ()
        {
            return new MTSpawn (ActionOne.Reverse (), ActionTwo.Reverse ());
        }
    }

    public class MTSpawnState : MTFiniteTimeActionState
    {

        protected MTFiniteTimeAction ActionOne { get; set; }

        private MTFiniteTimeActionState ActionStateOne { get; set; }

        protected MTFiniteTimeAction ActionTwo { get; set; }

        private MTFiniteTimeActionState ActionStateTwo { get; set; }

        public MTSpawnState (MTSpawn action, GameObject target)
            : base (action, target)
        { 
            ActionOne = action.ActionOne;
            ActionTwo = action.ActionTwo;

            ActionStateOne = (MTFiniteTimeActionState)ActionOne.StartAction (target);
            ActionStateTwo = (MTFiniteTimeActionState)ActionTwo.StartAction (target);
        }

        protected internal override void Stop ()
        {
            ActionStateOne.Stop ();
            ActionStateTwo.Stop ();

            base.Stop ();
        }

        public override void Update (float time)
        {
            if (ActionOne != null)
            {
                ActionStateOne.Update (time);
            }

            if (ActionTwo != null)
            {
                ActionStateTwo.Update (time);
            }
        }

    }

}
