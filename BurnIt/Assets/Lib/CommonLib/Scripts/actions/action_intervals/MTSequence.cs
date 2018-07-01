//using System.Diagnostics;

using UnityEngine;

namespace MTUnity.Actions
{
    public class MTSequence : MTFiniteTimeAction
    {
        public MTFiniteTimeAction[] Actions { get; private set; }


        #region Constructors

        public MTSequence (MTFiniteTimeAction action1, MTFiniteTimeAction action2) : base (action1.Duration + action2.Duration)
        {
            Actions = new MTFiniteTimeAction[2];
            InitMTSequence (action1, action2);
        }

        public MTSequence (params MTFiniteTimeAction[] actions) : base ()
        {

            Actions = new MTFiniteTimeAction[2];

            var prev = actions [0];

            // Can't call base(duration) because we need to calculate duration here
            float combinedDuration = 0.0f;

            for (int i = 0 ; i < actions.Length; ++i) 
            {
                var action = actions[i];
                combinedDuration += action.Duration;
            }

            Duration = combinedDuration;

            if (actions.Length == 1)
            {
                InitMTSequence (prev, new MTExtraAction ());
            }
            else
            {
                // Basically what we are doing here is creating a whole bunch of 
                // nested MTSequences from the actions.
                for (int i = 1; i < actions.Length - 1; i++)
                {
                    prev = new MTSequence (prev, actions [i]);
                }

                InitMTSequence (prev, actions [actions.Length - 1]);
            }

        }

        void InitMTSequence (MTFiniteTimeAction actionOne, MTFiniteTimeAction actionTwo)
        {
            Debug.Assert (actionOne != null);
            Debug.Assert (actionTwo != null);

            Actions [0] = actionOne;
            Actions [1] = actionTwo;
        }

        #endregion Constructors

        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTSequenceState (this, target);

        }

        public override MTFiniteTimeAction Reverse ()
        {
            return new MTSequence (Actions [1].Reverse (), Actions [0].Reverse ());
        }
    }

    public class MTSequenceState : MTFiniteTimeActionState
    {
        protected int last;
        protected MTFiniteTimeAction[] actionSequences = new MTFiniteTimeAction[2];
        protected MTFiniteTimeActionState[] actionStates = new MTFiniteTimeActionState[2];
        protected float split;
        private bool hasInfiniteAction = false;

        public MTSequenceState (MTSequence action, GameObject target)
            : base (action, target)
        { 
            actionSequences = action.Actions;
            hasInfiniteAction = (actionSequences [0] is MTRepeatForever) || (actionSequences [1] is MTRepeatForever);
            split = actionSequences [0].Duration / Duration;
            last = -1;

        }

        public override bool IsDone {
            get {
                if (hasInfiniteAction && actionSequences [last] is MTRepeatForever)
                {
                    return false;
                }

                return base.IsDone;
            }
		}

		public MTFiniteTimeAction Actions(int index) {
			if (index >= 0 && index < actionSequences.Length) {
				return actionSequences [index];
			}
			return null;
		}

		public MTFiniteTimeActionState ActionStates(int index) {
			if (index >= 0 && index < actionStates.Length) {
				return actionStates [index];
			}
			return null;
		}

        protected internal override void Stop ()
        {
            // Issue #1305
            if (last != -1)
            {
                actionStates [last].Stop ();
            }
        }

        protected internal override void Step (float dt)
        {
            if (last > -1 && (actionSequences [last] is MTRepeatForever))
            {
                actionStates [last].Step (dt);
            }
            else
            {
                base.Step (dt);
            }
        }

        public override void Update (float time)
        {
            int found;
            float new_t;

            if (time < split)
            {
                // action[0]
                found = 0;
                if (split != 0)
                    new_t = time / split;
                else
                    new_t = 1;
            }
            else
            {
                // action[1]
                found = 1;
                if (split == 1)
                    new_t = 1;
                else
                    new_t = (time - split) / (1 - split);
            }

            if (found == 1)
            {
                if (last == -1)
                {
                    // action[0] was skipped, execute it.
                    actionStates [0] = (MTFiniteTimeActionState)actionSequences [0].StartAction (Target);
                    actionStates [0].Update (1.0f);
                    actionStates [0].Stop ();
                }
                else if (last == 0)
                {
                    actionStates [0].Update (1.0f);
                    actionStates [0].Stop ();
                }
            }
            else if (found == 0 && last == 1)
            {
                // Reverse mode ?
                // XXX: Bug. this case doesn't contemplate when _last==-1, found=0 and in "reverse mode"
                // since it will require a hack to know if an action is on reverse mode or not.
                // "step" should be overriden, and the "reverseMode" value propagated to inner Sequences.
                actionStates [1].Update (0);
                actionStates [1].Stop ();

            }

            // Last action found and it is done.
            if (found == last && actionStates [found].IsDone)
            {
                return;
            }


            // Last action found and it is done
            if (found != last)
            {
                actionStates [found] = (MTFiniteTimeActionState)actionSequences [found].StartAction (Target);
            }

            actionStates [found].Update (new_t);
            last = found;

        }


    }

}