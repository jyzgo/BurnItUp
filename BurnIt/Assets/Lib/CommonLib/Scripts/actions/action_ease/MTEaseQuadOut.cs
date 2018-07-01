using UnityEngine;

namespace MTUnity.Actions
{
	public class MTEaseQuadOut : MTActionEase
	{
		public MTEaseQuadOut (MTFiniteTimeAction action) : base (action)
		{
		}

		protected internal override MTActionState StartAction (GameObject target)
		{
			return new MTEaseQuadOutState (this, target);
		}

		public override MTFiniteTimeAction Reverse ()
		{
			return new MTEaseQuadIn ((MTFiniteTimeAction)InnerAction.Reverse ());
		}
	}

	public class MTEaseQuadOutState : MTActionEaseState
	{
		public MTEaseQuadOutState (MTEaseQuadOut action, GameObject target) : base (action, target)
		{
		}

		public override void Update (float time)
		{
			InnerActionState.Update (MTEaseMath.QuadOut (time));
		}
	}

}