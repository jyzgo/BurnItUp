using UnityEngine;

namespace MTUnity.Actions
{
	public class MTEaseCircIn : MTActionEase
	{
		public MTEaseCircIn (MTFiniteTimeAction action) : base (action)
		{
		}

		protected internal override MTActionState StartAction (GameObject target)
		{
			return new MTEaseCircInState (this, target);
		}

		public override MTFiniteTimeAction Reverse ()
		{
			return new MTEaseCircOut ((MTFiniteTimeAction)InnerAction.Reverse ());
		}
	}

	public class MTEaseCircInState : MTActionEaseState
	{
		public MTEaseCircInState (MTEaseCircIn action, GameObject target) : base (action, target)
		{
		}

		public override void Update (float time)
		{
			InnerActionState.Update (MTEaseMath.CircIn (time));
		}
	}

}