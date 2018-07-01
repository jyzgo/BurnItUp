using UnityEngine;
using System.Collections;
using MTUnity.Actions;
using MTUnity.Utils;

namespace MTUnity.Actions
{
	public class MTSpineFadeIn : MTFiniteTimeAction
	{
		#region Constructors
		
		public MTSpineFadeIn (float durataion) : base (durataion)
		{
		}
		
		#endregion Constructors
		
		
		protected internal override MTActionState StartAction(GameObject target)
		{
			return new MTSpineFadeInState (this, target);
			
		}
		
		public override MTFiniteTimeAction Reverse ()
		{
			return new MTSpineFadeOut (Duration);
		}
	}
	
	public class MTSpineFadeInState : MTFiniteTimeActionState
	{
		
		protected uint Times { get; set; }
		
		protected bool OriginalState { get; set; }
		
		public MTSpineFadeInState (MTSpineFadeIn action, GameObject target)
			: base (action, target)
		{
		}
		
		public override void Update (float time)
		{
			var pRGBAProtocol = Target;
			if (pRGBAProtocol != null)
			{
				pRGBAProtocol.SetAlpha (time);
			}
		}
	}



	public class MTSpineFadeOut : MTFiniteTimeAction
	{
		#region Constructors
		
		public MTSpineFadeOut (float durtaion) : base (durtaion)
		{
		}
		
		#endregion Constructors
		
		protected internal override MTActionState StartAction(GameObject target)
		{
			return new MTSpineFadeOutState (this, target);
			
		}
		
		public override MTFiniteTimeAction Reverse ()
		{
			return new MTSpineFadeIn (Duration);
		}
	}
	
	public class MTSpineFadeOutState : MTFiniteTimeActionState
	{
		
		public MTSpineFadeOutState (MTSpineFadeOut action, GameObject target)
			: base (action, target)
		{
		}
		
		public override void Update (float time)
		{
			var pRGBAProtocol = Target;
			if (pRGBAProtocol != null)
			{
				pRGBAProtocol.SetAlpha (1 - time);
			}
		}
		
	}

}
