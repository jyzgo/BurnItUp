using UnityEngine;

namespace MTUnity.Actions
{
	public class MTEaseCubicOut : MTActionEase
	{
		public MTEaseCubicOut (MTFiniteTimeAction action) : base (action)
		{
		}

		protected internal override MTActionState StartAction (GameObject target)
		{
			return new MTEaseCubicOutState (this, target);
		}

		public override MTFiniteTimeAction Reverse ()
		{
			return new MTEaseCubicIn ((MTFiniteTimeAction)InnerAction.Reverse ());
		}
	}

	public class MTEaseCubicOutState : MTActionEaseState
	{
		public MTEaseCubicOutState (MTEaseCubicOut action, GameObject target) : base (action, target)
		{
		}

		public override void Update (float time)
		{
			InnerActionState.Update (MTEaseMath.CubicOut (time));
		}
	}

}