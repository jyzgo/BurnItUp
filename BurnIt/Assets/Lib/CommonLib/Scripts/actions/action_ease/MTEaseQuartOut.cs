using UnityEngine;

namespace MTUnity.Actions
{
	public class MTEaseQuartOut : MTActionEase
	{
		public MTEaseQuartOut (MTFiniteTimeAction action) : base (action)
		{
		}

		protected internal override MTActionState StartAction (GameObject target)
		{
			return new MTEaseQuartOutState (this, target);
		}

		public override MTFiniteTimeAction Reverse ()
		{
			return new MTEaseQuartIn ((MTFiniteTimeAction)InnerAction.Reverse ());
		}
	}

	public class MTEaseQuartOutState : MTActionEaseState
	{
		public MTEaseQuartOutState (MTEaseQuartOut action, GameObject target) : base (action, target)
		{
		}

		public override void Update (float time)
		{
			InnerActionState.Update (MTEaseMath.QuartOut (time));
		}
	}

}