using UnityEngine;

namespace MTUnity.Actions
{
    public class MTRotateBy : MTFiniteTimeAction
    {
        public float AngleX { get; private set; }
        public float AngleY { get; private set; }
		public float AngleZ { get;private set;}


        #region Constructors

		public MTRotateBy (float duration, float deltaAngleX, float deltaAngleY,float deltaAngleZ) : base (duration)
        {
            AngleX = deltaAngleX;
            AngleY = deltaAngleY;
			AngleZ = deltaAngleZ;
        }

		public MTRotateBy (float duration, float deltaAngle) : this (duration, deltaAngle, deltaAngle,deltaAngle)
        {
        }

        #endregion Constructors

        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTRotateByState (this, target);

        }

        public override MTFiniteTimeAction Reverse ()
        {
			return new MTRotateBy (Duration, -AngleX, -AngleY,-AngleZ);
        }
    }

    public class MTRotateByState : MTFiniteTimeActionState
    {

        protected float AngleX { get; set; }

        protected float AngleY { get; set; }

		protected float AngleZ { get;set;}

        protected float StartAngleX { get; set; }

        protected float StartAngleY { get; set; }

		protected float StartAngleZ { get; set;}

        public MTRotateByState (MTRotateBy action, GameObject target)
            : base (action, target)
        { 
            AngleX = action.AngleX;
            AngleY = action.AngleY;
			AngleZ = action.AngleZ;

			StartAngleX = target.transform.localRotation.x;
			StartAngleY = target.transform.localRotation.y;
			StartAngleZ = target.transform.localRotation.z;

        }

        public override void Update (float time)
        {
            // XXX: shall I add % 360
            if (Target != null)
            {
				var RotationX = StartAngleX + AngleX * time;
                var RotationY = StartAngleY + AngleY * time;
				var RotationZ = StartAngleZ + AngleZ * time;

				Target.transform.Rotate (new Vector3 (RotationX, RotationY, RotationZ));
            }
        }

    }

}