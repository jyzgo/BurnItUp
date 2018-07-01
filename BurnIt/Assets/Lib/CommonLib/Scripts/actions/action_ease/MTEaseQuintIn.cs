using UnityEngine;

namespace MTUnity.Actions
{
	public class MTEaseQuintIn : MTActionEase
	{
		public MTEaseQuintIn (MTFiniteTimeAction action) : base (action)
		{
		}

		protected internal override MTActionState StartAction (GameObject target)
		{
			return new MTEaseQuintInState (this, target);
		}

		public override MTFiniteTimeAction Reverse ()
		{
			return new MTEaseQuintOut ((MTFiniteTimeAction)InnerAction.Reverse ());
		}
	}

	public class MTEaseQuintInState : MTActionEaseState
	{
		public MTEaseQuintInState (MTEaseQuintIn action, GameObject target) : base (action, target)
		{
		}

		public override void Update (float time)
		{
			InnerActionState.Update (MTEaseMath.QuintIn (time));
		}
	}

}