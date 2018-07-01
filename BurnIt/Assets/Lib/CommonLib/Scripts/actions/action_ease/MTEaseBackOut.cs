using UnityEngine;

namespace MTUnity.Actions
{
    public class MTEaseBackOut : MTActionEase
    {
        #region Constructors

        public MTEaseBackOut (MTFiniteTimeAction action) : base (action)
        {
        }

        #endregion Constructors


        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTEaseBackOutState (this, target);
        }

        public override MTFiniteTimeAction Reverse ()
        {
            return new MTEaseBackIn ((MTFiniteTimeAction)InnerAction.Reverse ());
        }
    }


    #region Action state

    public class MTEaseBackOutState : MTActionEaseState
    {
        public MTEaseBackOutState (MTEaseBackOut action, GameObject target) : base (action, target)
        {
        }

        public override void Update (float time)
        {
            InnerActionState.Update (MTEaseMath.BackOut (time));
        }
    }

    #endregion Action state
}