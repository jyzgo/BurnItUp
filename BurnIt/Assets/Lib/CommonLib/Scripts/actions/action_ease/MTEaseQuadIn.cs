using UnityEngine;

namespace MTUnity.Actions
{
	public class MTEaseQuadIn : MTActionEase
	{
		public MTEaseQuadIn (MTFiniteTimeAction action) : base (action)
		{
		}

		protected internal override MTActionState StartAction (GameObject target)
		{
			return new MTEaseQuadInState (this, target);
		}

		public override MTFiniteTimeAction Reverse ()
		{
			return new MTEaseQuadOut ((MTFiniteTimeAction)InnerAction.Reverse ());
		}
	}

	public class MTEaseQuadInState : MTActionEaseState
	{
		public MTEaseQuadInState (MTEaseQuadIn action, GameObject target) : base (action, target)
		{
		}

		public override void Update (float time)
		{
			InnerActionState.Update (MTEaseMath.QuadIn (time));
		}
	}

}