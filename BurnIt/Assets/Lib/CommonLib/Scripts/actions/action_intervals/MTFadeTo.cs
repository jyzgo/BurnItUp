using System;

using UnityEngine;

namespace MTUnity.Actions
{
    public class MTFadeTo : MTFiniteTimeAction
    {
        public float ToOpacity { get; private set; }


        #region Constructors

        public MTFadeTo (float duration, float opacity) : base (duration)
        {
            ToOpacity = opacity;
        }

        #endregion Constructors


        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTFadeToState (this, target);

        }

        public override MTFiniteTimeAction Reverse()
        {
            throw new NotImplementedException();
        }
    }

    public class MTFadeToState : MTFiniteTimeActionState
    {
		protected float FromOpacity { get; set; }

		protected float ToOpacity { get; set; }

        public MTFadeToState (MTFadeTo action, GameObject target)
            : base (action, target)
        {              
            ToOpacity = action.ToOpacity;

            var pRGBAProtocol = target;
            if (pRGBAProtocol != null)
            {
				FromOpacity = pRGBAProtocol.getOpacity();
            }
        }

        public override void Update (float time)
        {
            var pRGBAProtocol = Target;
            if (pRGBAProtocol != null)
            {
				pRGBAProtocol.setOpacity (FromOpacity + (ToOpacity - FromOpacity) * time);
            }
        }
    }


}