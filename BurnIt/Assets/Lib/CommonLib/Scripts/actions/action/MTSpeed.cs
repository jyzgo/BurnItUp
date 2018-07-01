using UnityEngine;

namespace MTUnity.Actions
{
    public class MTSpeed : MTAction
    {
        public float Speed { get; private set; }

        protected internal MTFiniteTimeAction InnerAction { get; private set; }


        #region Constructors

        public MTSpeed (MTFiniteTimeAction action, float speed)
        {
            InnerAction = action;
            Speed = speed;
        }

        #endregion Constructors


        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTSpeedState (this, target);
        }

        public virtual MTFiniteTimeAction Reverse ()
        {
            return (MTFiniteTimeAction)(MTAction)new MTSpeed ((MTFiniteTimeAction)InnerAction.Reverse(), Speed);
        }
    }


    #region Action state

    internal class MTSpeedState : MTActionState
    {
        #region Properties

        public float Speed { get; private set; }

        protected MTFiniteTimeActionState InnerActionState { get; private set; }

        public override bool IsDone 
        {
            get { return InnerActionState.IsDone; }
        }

        #endregion Properties


        public MTSpeedState (MTSpeed action, GameObject target) : base (action, target)
        {
            InnerActionState = (MTFiniteTimeActionState)action.InnerAction.StartAction (target);
            Speed = action.Speed;
        }

        protected internal override void Stop ()
        {
            InnerActionState.Stop ();
            base.Stop ();
        }

        protected internal override void Step (float dt)
        {
            InnerActionState.Step (dt * Speed);
        }
    }

    #endregion Action state
}