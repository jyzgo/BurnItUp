using System;
using UnityEngine;


namespace MTUnity.Actions
{
    public abstract class MTAmplitudeAction : MTFiniteTimeAction
    {
        public float Amplitude { get; private set; }


        #region Constructors

        public MTAmplitudeAction (float duration, float amplitude = 0) : base (duration)
        {
            Amplitude = amplitude;
        }

        #endregion Constructors
    }


    #region Action state

    public abstract class MTAmplitudeActionState : MTFiniteTimeActionState
    {
        protected float Amplitude { get; private set; }
        protected internal float AmplitudeRate { get; set; }

        public MTAmplitudeActionState (MTAmplitudeAction action, GameObject target) : base (action, target)
        {
            Amplitude = action.Amplitude;
            AmplitudeRate = 1.0f;
        }
    }

    #endregion Action state
}
