using UnityEngine;

namespace MTUnity.Actions
{
    public class MTScaleTo : MTFiniteTimeAction
    {
        public float EndScaleX { get; private set; }
        public float EndScaleY { get; private set; }
		public float EndScaleZ { get; private set; }


        #region Constructors

		public MTScaleTo (float duration, float scale) : this (duration, scale, scale,scale)
        {
		}

		public MTScaleTo (float duration, Vector3 scale) : this (duration, scale.x, scale.y, scale.z)
		{
		}

		public MTScaleTo (float duration, float scaleX, float scaleY,float scaleZ) : base (duration)
        {
            EndScaleX = scaleX;
            EndScaleY = scaleY;
			EndScaleZ = scaleZ;
        }

        #endregion Constructors

        public override MTFiniteTimeAction Reverse()
        {
            throw new System.NotImplementedException ();
        }

        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTScaleToState (this, target);
        }
    }

    public class MTScaleToState : MTFiniteTimeActionState
    {
        protected float DeltaX;
        protected float DeltaY;
		protected float DeltaZ;

        protected float EndScaleX;
        protected float EndScaleY;
		protected float EndScaleZ;

        protected float StartScaleX;
        protected float StartScaleY;
		protected float StartScaleZ;

        public MTScaleToState (MTScaleTo action, GameObject target)
            : base (action, target)
        { 
			if(target == null)
			{
				return;
			}
			StartScaleX = target.transform.localScale.x;
			StartScaleY = target.transform.localScale.y;
			StartScaleZ = target.transform.localScale.z;

            EndScaleX = action.EndScaleX;
            EndScaleY = action.EndScaleY;
			EndScaleZ = action.EndScaleZ;

            DeltaX = EndScaleX - StartScaleX;
            DeltaY = EndScaleY - StartScaleY;
			DeltaZ = EndScaleZ - StartScaleZ;
        }

        public override void Update (float time)
        {
            if (Target != null)
            {
               	var ScaleX = StartScaleX + DeltaX * time;
                var ScaleY = StartScaleY + DeltaY * time;
				var ScaleZ = StartScaleZ + DeltaZ * time;
				Target.transform.localScale = new Vector3 (ScaleX, ScaleY, ScaleZ);
            }
        }
    }
}