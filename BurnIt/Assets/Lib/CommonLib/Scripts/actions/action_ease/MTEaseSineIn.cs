using System;
//using Microsoft.Xna.Framework;



using UnityEngine;

namespace MTUnity.Actions
{
	public class MTEaseSineIn : MTActionEase
	{
		#region Constructors

        public MTEaseSineIn (MTFiniteTimeAction action) : base (action)
		{
		}

		#endregion Constructors


		protected internal override MTActionState StartAction(GameObject target)
		{
			return new MTEaseSineInState (this, target);
		}

		public override MTFiniteTimeAction Reverse ()
		{
            return new MTEaseSineOut ((MTFiniteTimeAction)InnerAction.Reverse ());
		}
	}


	#region Action state

	public class MTEaseSineInState : MTActionEaseState
	{
		public MTEaseSineInState (MTEaseSineIn action, GameObject target) : base (action, target)
		{
		}

		public override void Update (float time)
		{
			InnerActionState.Update (MTEaseMath.SineIn (time));
		}
	}

	#endregion Action state
}