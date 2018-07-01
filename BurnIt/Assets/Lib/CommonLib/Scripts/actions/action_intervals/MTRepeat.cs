using UnityEngine;

namespace MTUnity.Actions
{
    public class MTRepeat : MTFiniteTimeAction
    {
        #region Properties

        public bool ActionInstant { get; private set; }
        public uint Times { get; private set; }
        public uint Total { get; private set; }
        public MTFiniteTimeAction InnerAction { get; private set; }

        #endregion Properties


        #region Constructors

        public MTRepeat (MTFiniteTimeAction action, uint times) : base (action.Duration * times)
        {

            Times = times;
            InnerAction = action;

            ActionInstant = action is MTActionInstant;
            //an instant action needs to be executed one time less in the update method since it uses startWithTarget to execute the action
            if (ActionInstant)
            {
                Times -= 1;
            }
            Total = 0;
        }

        #endregion Constructors

        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTRepeatState (this, target);

        }

        public override MTFiniteTimeAction Reverse ()
        {
            return new MTRepeat (InnerAction.Reverse(), Times);
        }
    }

    public class MTRepeatState : MTFiniteTimeActionState
    {

        protected bool ActionInstant { get; set; }

        protected float NextDt { get; set; }

        protected MTFiniteTimeAction InnerAction { get; set; }

        protected MTFiniteTimeActionState InnerActionState { get; set; }

        protected uint Times { get; set; }

        protected uint Total { get; set; }

        public MTRepeatState (MTRepeat action, GameObject target)
            : base (action, target)
        { 

            InnerAction = action.InnerAction;
            Times = action.Times;
            Total = action.Total;
            ActionInstant = action.ActionInstant;

            NextDt = InnerAction.Duration / Duration;

            InnerActionState = (MTFiniteTimeActionState)InnerAction.StartAction (target);
        }

        protected internal override void Stop ()
        {
            InnerActionState.Stop ();
            base.Stop ();
        }

        // issue #80. Instead of hooking step:, hook update: since it can be called by any
        // container action like Repeat, Sequence, AccelDeccel, etc..
        public override void Update (float time)
        {
            if (time >= NextDt)
            {
                while (time > NextDt && Total < Times)
                {
                    InnerActionState.Update (1.0f);
                    Total++;

                    InnerActionState.Stop ();
                    InnerActionState = (MTFiniteTimeActionState)InnerAction.StartAction (Target);
                    NextDt = InnerAction.Duration / Duration * (Total+1f);
                }

                // fix for issue #1288, incorrect end value of repeat
                if (time >= 1.0f && Total < Times)
                {
                    Total++;
                }

                // don't set an instant action back or update it, it has no use because it has no duration
                if (!ActionInstant)
                {
                    if (Total == Times)
                    {
                        InnerActionState.Update (1f);
                        InnerActionState.Stop ();
                    }
                    else
                    {
                        // issue #390 prevent jerk, use right update
                        InnerActionState.Update (time - (NextDt - InnerAction.Duration / Duration));
                    }

                }
            }
            else
            {
                InnerActionState.Update ((time * Times) % 1.0f);
            }
        }

        public override bool IsDone {
            get { return Total == Times; }
        }

    }

}