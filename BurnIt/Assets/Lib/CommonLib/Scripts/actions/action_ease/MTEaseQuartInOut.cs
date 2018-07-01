using UnityEngine;

namespace MTUnity.Actions
{
	public class MTEaseQuartInOut : MTActionEase
	{
		public MTEaseQuartInOut (MTFiniteTimeAction action) : base (action)
		{
		}

		protected internal override MTActionState StartAction (GameObject target)
		{
			return new MTEaseQuartInOutState (this, target);
		}

		public override MTFiniteTimeAction Reverse ()
		{
			return new MTEaseQuartInOut ((MTFiniteTimeAction)InnerAction.Reverse ());
		}
	}

	public class MTEaseQuartInOutState : MTActionEaseState
	{
		public MTEaseQuartInOutState (MTEaseQuartInOut action, GameObject target) : base (action, target)
		{
		}

		public override void Update (float time)
		{
			InnerActionState.Update (MTEaseMath.QuartInOut (time));
		}
	}

}