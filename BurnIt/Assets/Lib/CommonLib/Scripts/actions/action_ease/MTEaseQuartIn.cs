using UnityEngine;

namespace MTUnity.Actions
{
	public class MTEaseQuartIn : MTActionEase
	{
		public MTEaseQuartIn (MTFiniteTimeAction action) : base (action)
		{
		}

		protected internal override MTActionState StartAction (GameObject target)
		{
			return new MTEaseQuartInState (this, target);
		}

		public override MTFiniteTimeAction Reverse ()
		{
			return new MTEaseQuartOut ((MTFiniteTimeAction)InnerAction.Reverse ());
		}
	}

	public class MTEaseQuartInState : MTActionEaseState
	{
		public MTEaseQuartInState (MTEaseQuartIn action, GameObject target) : base (action, target)
		{
		}

		public override void Update (float time)
		{
			InnerActionState.Update (MTEaseMath.QuartIn (time));
		}
	}

}