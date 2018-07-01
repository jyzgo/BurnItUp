using UnityEngine;

namespace MTUnity.Actions
{
	public class MTScaleBy : MTScaleTo
	{
		#region Constructors


        public MTScaleBy (float duration, float scale) : base (duration, scale)
		{
		}

		public MTScaleBy (float duration, float scaleX, float scaleY,float scaleZ) : base (duration, scaleX, scaleY,scaleZ)
		{
		}

		#endregion Constructors

		protected internal override MTActionState StartAction(GameObject target)
		{
			return new MTScaleByState (this, target);

		}

		public override MTFiniteTimeAction Reverse ()
		{
			return new MTScaleBy (Duration, 1 / EndScaleX, 1 / EndScaleY , 1/EndScaleZ);
		}

	}

    public class MTScaleByState : MTScaleToState
	{

		public MTScaleByState (MTScaleTo action, GameObject target)
			: base (action, target)
		{ 
			DeltaX = StartScaleX * EndScaleX - StartScaleX;
			DeltaY = StartScaleY * EndScaleY - StartScaleY;
			DeltaZ = StartScaleZ * EndScaleZ - StartScaleZ;
		}

	}

}