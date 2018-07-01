using System;
using UnityEngine;

namespace MTUnity.Actions
{
    public abstract class MTFiniteTimeAction : MTAction
    {
        float duration;

        #region Properties

        public virtual float Duration 
        {
            get 
            {
                return duration;
            }
            set 
            {
                float newDuration = value;

                // Prevent division by 0
                if (newDuration == 0)
                {
                    newDuration = float.Epsilon;
                }

                duration = newDuration;
            }
        }

        #endregion Properties


        #region Constructors

        protected MTFiniteTimeAction() 
            : this (0)
        {
        }

        protected MTFiniteTimeAction (float duration)
        {
            Duration = duration;
        }

        #endregion Constructors


        public abstract MTFiniteTimeAction Reverse();

        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTFiniteTimeActionState (this, target);
        }
    }

    public class MTFiniteTimeActionState : MTActionState
    {
        bool firstTick;

        #region Properties

        public virtual float Duration { get; set; }
        public float Elapsed { get; private set; }

        public override bool IsDone 
        {
            get { return Elapsed >= Duration; }
        }

        #endregion Properties


        public MTFiniteTimeActionState (MTFiniteTimeAction action, GameObject target)
            : base (action, target)
        { 
            Duration = action.Duration;
            Elapsed = 0.0f;
            firstTick = true;
        }

        protected internal override void Step(float dt)
        {
            if (firstTick)
            {
                firstTick = false;
                Elapsed = 0f;
            }
            else
            {
                Elapsed += dt;
            }

            Update (Math.Max (0f,
                Math.Min (1, Elapsed / Math.Max (Duration, float.Epsilon)
                )
            )
            );
        }

    }
}