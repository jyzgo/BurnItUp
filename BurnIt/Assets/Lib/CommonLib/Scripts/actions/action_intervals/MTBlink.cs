using UnityEngine;

namespace MTUnity.Actions
{
    public class MTBlink : MTFiniteTimeAction
    {
        public uint Times { get; private set; }


        #region Constructors

        public MTBlink (float duration, uint numOfBlinks) : base (duration)
        {
            Times = numOfBlinks;
        }

        #endregion Constructors


        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTBlinkState (this, target);

        }

        public override MTFiniteTimeAction Reverse ()
        {
            return new MTBlink (Duration, Times);
        }
    }

    public class MTBlinkState : MTFiniteTimeActionState
    {

        protected uint Times { get; set; }

        protected bool OriginalState { get; set; }

        public MTBlinkState (MTBlink action, GameObject target)
            : base (action, target)
        { 
            Times = action.Times;
			OriginalState = target.getVisible();
        }

        public override void Update (float time)
        {
            if (Target != null && !IsDone)
            {
                float slice = 1.0f / Times;
                // float m = fmodf(time, slice);
                float m = time % slice;
				Target.setVisible( m > (slice / 2));
            }
        }

        protected internal override void Stop ()
        {
			Target.setVisible(OriginalState);
            base.Stop ();
        }

    }
}