using UnityEngine;

namespace MTUnity.Actions
{
	public class MTEaseQuintOut : MTActionEase
	{
		public MTEaseQuintOut (MTFiniteTimeAction action) : base (action)
		{
		}

		protected internal override MTActionState StartAction (GameObject target)
		{
			return new MTEaseQuintOutState (this, target);
		}

		public override MTFiniteTimeAction Reverse ()
		{
			return new MTEaseQuintIn ((MTFiniteTimeAction)InnerAction.Reverse ());
		}
	}

	public class MTEaseQuintOutState : MTActionEaseState
	{
		public MTEaseQuintOutState (MTEaseQuintOut action, GameObject target) : base (action, target)
		{
		}

		public override void Update (float time)
		{
			InnerActionState.Update (MTEaseMath.QuintOut (time));
		}
	}

}