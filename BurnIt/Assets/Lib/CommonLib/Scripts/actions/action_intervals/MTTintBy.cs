using UnityEngine;

namespace MTUnity.Actions
{

    public class MTTintBy : MTFiniteTimeAction
    {
        public float DeltaB { get; private set; }
        public float DeltaG { get; private set; }
        public float DeltaR { get; private set; }


        #region Constructors

        public MTTintBy (float duration, float deltaRed, float deltaGreen, float deltaBlue) : base (duration)
        {
            DeltaR = deltaRed;
            DeltaG = deltaGreen;
            DeltaB = deltaBlue;
        }

        #endregion Constructors


        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTTintByState (this, target);
        }

        public override MTFiniteTimeAction Reverse ()
        {
            return new MTTintBy (Duration, (float)-DeltaR, (float)-DeltaG, (float)-DeltaB);
        }
    }


    public class MTTintByState : MTFiniteTimeActionState
    {
        protected float DeltaB { get; set; }

        protected float DeltaG { get; set; }

        protected float DeltaR { get; set; }

        protected float FromB { get; set; }

        protected float FromG { get; set; }

        protected float FromR { get; set; }

        public MTTintByState (MTTintBy action, GameObject target)
            : base (action, target)
        {   
            DeltaB = action.DeltaB;
            DeltaG = action.DeltaG;
            DeltaR = action.DeltaR;

            var protocol = target;
            if (protocol != null)
            {
				var color = protocol.getColor();
                FromR = color.r;
                FromG = color.g;
                FromB = color.b;
            }
        }

        public override void Update (float time)
        {
            var protocol = Target;
            if (protocol != null)
            {
				var newColor = new Color ((FromR + DeltaR * time),
                    (FromG + DeltaG * time),
                    (FromB + DeltaB * time));


				protocol.setColor (newColor); 
            }
        }

    }

}