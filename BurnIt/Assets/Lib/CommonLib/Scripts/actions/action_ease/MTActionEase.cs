using UnityEngine;

namespace MTUnity.Actions
{
    public class MTActionEase : MTFiniteTimeAction
    {
        protected internal MTFiniteTimeAction InnerAction { get; private set; }


        #region Constructors

        public MTActionEase(MTFiniteTimeAction action) : base (action.Duration)
        {
            InnerAction = action;
        }

        #endregion Constructors


        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTActionEaseState (this, target);
        }

        public override MTFiniteTimeAction Reverse()
        {
            return new MTActionEase ((MTFiniteTimeAction)InnerAction.Reverse ());
        }
    }


    #region Action state

    public class MTActionEaseState : MTFiniteTimeActionState
    {
        protected MTFiniteTimeActionState InnerActionState { get; private set; }

        public MTActionEaseState (MTActionEase action, GameObject target) : base (action, target)
        {
            InnerActionState = (MTFiniteTimeActionState)action.InnerAction.StartAction (target);
        }

        protected internal override void Stop ()
        {
            InnerActionState.Stop ();
            base.Stop ();
        }

        public override void Update (float time)
        {
            InnerActionState.Update (time);
        }
    }

    #endregion Action state
}