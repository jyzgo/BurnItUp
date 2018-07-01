using UnityEngine;

namespace MTUnity.Actions
{
    public class MTTintTo : MTFiniteTimeAction
    {
        public Color ColorTo { get; private set; }


        #region Constructors

		public MTTintTo (float duration, float red, float green, float blue) : base (duration)
        {
            ColorTo = new Color (red, green, blue);
        }

        #endregion Constructors

        public override MTFiniteTimeAction Reverse()
        {
            throw new System.NotImplementedException ();
        }

        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTTintToState (this, target);
        }
    }

    public class MTTintToState : MTFiniteTimeActionState
    {
        protected Color ColorFrom { get; set; }

        protected Color ColorTo { get; set; }

        public MTTintToState (MTTintTo action, GameObject target)
            : base (action, target)
        {   
            ColorTo = action.ColorTo;
            var protocol = Target;
            if (protocol != null)
            {
				ColorFrom = protocol.getColor();
            }
        }

        public override void Update (float time)
        {
            var protocol = Target;
            if (protocol != null)
            {
				
				protocol.GetComponent<Renderer> ().material.color = new Color ((ColorFrom.r + (ColorTo.r - ColorFrom.r) * time),
                    (ColorFrom.g + (ColorTo.g - ColorFrom.g) * time),
                    (ColorFrom.b + (ColorTo.b - ColorFrom.b) * time));
            }
        }

    }

}