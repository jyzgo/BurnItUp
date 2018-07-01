using UnityEngine;

namespace MTUnity.Actions
{
	public class MTEaseCubicInOut : MTActionEase
	{
		public MTEaseCubicInOut (MTFiniteTimeAction action) : base (action)
		{
		}

		protected internal override MTActionState StartAction (GameObject target)
		{
			return new MTEaseCubicInOutState (this, target);
		}

		public override MTFiniteTimeAction Reverse ()
		{
			return new MTEaseCubicInOut ((MTFiniteTimeAction)InnerAction.Reverse ());
		}
	}

	public class MTEaseCubicInOutState : MTActionEaseState
	{
		public MTEaseCubicInOutState (MTEaseCubicInOut action, GameObject target) : base (action, target)
		{
		}

		public override void Update (float time)
		{
			InnerActionState.Update (MTEaseMath.CubicInOut (time));
		}
	}

}