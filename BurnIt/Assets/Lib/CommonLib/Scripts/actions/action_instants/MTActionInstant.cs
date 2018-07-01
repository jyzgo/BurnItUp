using UnityEngine;

namespace MTUnity.Actions
{
    public class MTActionInstant : MTFiniteTimeAction
    {

        #region Constructors

        protected MTActionInstant ()
        {
        }

        #endregion Constructors

        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTActionInstantState (this, target);

        }

        public override MTFiniteTimeAction Reverse ()
        {
            return new MTActionInstant ();
        }
    }

    public class MTActionInstantState : MTFiniteTimeActionState
    {

        public MTActionInstantState (MTActionInstant action, GameObject target)
            : base (action, target)
        {
        }

        public override bool IsDone 
        {
            get { return true; }
        }

        protected internal override void Step (float dt)
        {
            Update (1);
        }

        public override void Update (float time)
        {
            // ignore
        }
    }

}