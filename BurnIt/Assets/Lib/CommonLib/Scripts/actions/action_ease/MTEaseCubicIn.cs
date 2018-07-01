using UnityEngine;

namespace MTUnity.Actions
{
	public class MTEaseCubicIn : MTActionEase
	{
		public MTEaseCubicIn (MTFiniteTimeAction action) : base (action)
		{
		}

		protected internal override MTActionState StartAction (GameObject target)
		{
			return new MTEaseCubicInState (this, target);
		}

		public override MTFiniteTimeAction Reverse ()
		{
			return new MTEaseCubicOut ((MTFiniteTimeAction)InnerAction.Reverse ());
		}
	}

	public class MTEaseCubicInState : MTActionEaseState
	{
		public MTEaseCubicInState (MTEaseCubicIn action, GameObject target) : base (action, target)
		{
		}

		public override void Update (float time)
		{
			InnerActionState.Update (MTEaseMath.CubicIn (time));
		}
	}

}