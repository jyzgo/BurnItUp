using UnityEngine;

namespace MTUnity.Actions
{
    public class MTEaseBackIn : MTActionEase
    {
        #region Constructors

        public MTEaseBackIn (MTFiniteTimeAction action) : base (action)
        {
        }

        #endregion Constructors


        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTEaseBackInState (this, target);
        }

        public override MTFiniteTimeAction Reverse ()
        {
            return new MTEaseBackOut ((MTFiniteTimeAction)InnerAction.Reverse ());
        }
    }


    #region Action state

    public class MTEaseBackInState : MTActionEaseState
    {
        public MTEaseBackInState (MTEaseBackIn action, GameObject target) : base (action, target)
        {
        }

        public override void Update (float time)
        {
            InnerActionState.Update (MTEaseMath.BackIn (time));
        }
    }

    #endregion Action state
}