using UnityEngine;

namespace MTUnity.Actions
{
	public class MTEaseQuintInOut : MTActionEase
	{
		public MTEaseQuintInOut (MTFiniteTimeAction action) : base (action)
		{
		}

		protected internal override MTActionState StartAction (GameObject target)
		{
			return new MTEaseQuintInOutState (this, target);
		}

		public override MTFiniteTimeAction Reverse ()
		{
			return new MTEaseQuintInOut ((MTFiniteTimeAction)InnerAction.Reverse ());
		}
	}

	public class MTEaseQuintInOutState : MTActionEaseState
	{
		public MTEaseQuintInOutState (MTEaseQuintInOut action, GameObject target) : base (action, target)
		{
		}

		public override void Update (float time)
		{
			InnerActionState.Update (MTEaseMath.QuintInOut (time));
		}
	}

}