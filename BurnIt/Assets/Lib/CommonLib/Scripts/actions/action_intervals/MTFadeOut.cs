using UnityEngine;

namespace MTUnity.Actions
{
    public class MTFadeOut : MTFiniteTimeAction
    {
        #region Constructors

        public MTFadeOut (float durtaion) : base (durtaion)
        {
        }

        #endregion Constructors

        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTFadeOutState (this, target);

        }

        public override MTFiniteTimeAction Reverse ()
        {
            return new MTFadeIn (Duration);
        }
    }

    public class MTFadeOutState : MTFiniteTimeActionState
    {

        public MTFadeOutState (MTFadeOut action, GameObject target)
            : base (action, target)
        {
        }

        public override void Update (float time)
        {
            var pRGBAProtocol = Target;
            if (pRGBAProtocol != null)
            {
				pRGBAProtocol.setOpacity(1 - time);
            }
        }

    }

}