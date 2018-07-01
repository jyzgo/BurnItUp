using UnityEngine;

namespace MTUnity.Actions
{
    public class MTFadeIn : MTFiniteTimeAction
    {
        #region Constructors

        public MTFadeIn (float durataion) : base (durataion)
        {
        }

        #endregion Constructors


        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTFadeInState (this, target);

        }

        public override MTFiniteTimeAction Reverse ()
        {
            return new MTFadeOut (Duration);
        }
    }

    public class MTFadeInState : MTFiniteTimeActionState
    {

        protected uint Times { get; set; }

        protected bool OriginalState { get; set; }

        public MTFadeInState (MTFadeIn action, GameObject target)
            : base (action, target)
        {
        }

        public override void Update (float time)
        {
            var pRGBAProtocol = Target;
            if (pRGBAProtocol != null)
            {
//                pRGBAProtocol.Opacity = (byte)(255 * time);
				pRGBAProtocol.setOpacity(time);
            }
        }
    }

}