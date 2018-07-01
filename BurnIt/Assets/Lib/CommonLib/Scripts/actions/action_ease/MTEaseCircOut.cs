using UnityEngine;

namespace MTUnity.Actions
{
	public class MTEaseCircOut : MTActionEase
	{
		public MTEaseCircOut (MTFiniteTimeAction action) : base (action)
		{
		}

		protected internal override MTActionState StartAction (GameObject target)
		{
			return new MTEaseCircOutState (this, target);
		}

		public override MTFiniteTimeAction Reverse ()
		{
			return new MTEaseCircIn ((MTFiniteTimeAction)InnerAction.Reverse ());
		}
	}

	public class MTEaseCircOutState : MTActionEaseState
	{
		public MTEaseCircOutState (MTEaseCircOut action, GameObject target) : base (action, target)
		{
		}

		public override void Update (float time)
		{
			InnerActionState.Update (MTEaseMath.CircOut (time));
		}
	}

}