using UnityEngine;

namespace MTUnity.Actions
{
	public class MTEaseCircInOut : MTActionEase
	{
		public MTEaseCircInOut (MTFiniteTimeAction action) : base (action)
		{
		}

		protected internal override MTActionState StartAction (GameObject target)
		{
			return new MTEaseCircInOutState (this, target);
		}

		public override MTFiniteTimeAction Reverse ()
		{
			return new MTEaseCircInOut ((MTFiniteTimeAction)InnerAction.Reverse ());
		}
	}

	public class MTEaseCircInOutState : MTActionEaseState
	{
		public MTEaseCircInOutState (MTEaseCircInOut action, GameObject target) : base (action, target)
		{
		}

		public override void Update (float time)
		{
			InnerActionState.Update (MTEaseMath.CircInOut (time));
		}
	}

}