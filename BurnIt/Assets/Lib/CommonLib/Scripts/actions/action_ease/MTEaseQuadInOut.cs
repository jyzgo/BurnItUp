using UnityEngine;

namespace MTUnity.Actions
{
	public class MTEaseQuadInOut : MTActionEase
	{
		public MTEaseQuadInOut (MTFiniteTimeAction action) : base (action)
		{
		}

		protected internal override MTActionState StartAction (GameObject target)
		{
			return new MTEaseQuadInOutState (this, target);
		}

		public override MTFiniteTimeAction Reverse ()
		{
			return new MTEaseQuadInOut ((MTFiniteTimeAction)InnerAction.Reverse ());
		}
	}

	public class MTEaseQuadInOutState : MTActionEaseState
	{
		public MTEaseQuadInOutState (MTEaseQuadInOut action, GameObject target) : base (action, target)
		{
		}

		public override void Update (float time)
		{
			InnerActionState.Update (MTEaseMath.QuadInOut (time));
		}
	}

}