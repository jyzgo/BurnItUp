using UnityEngine;

namespace MTUnity.Actions
{
	public class MTScaleParticleTo : MTFiniteTimeAction
	{
		public float EndScale { get; private set; }
		public float StartScale{get; private set;}
				
		
		#region Constructors
		
		public MTScaleParticleTo (float duration, float startScale,float endScale) 
		{
			EndScale = endScale;
			StartScale = startScale;
		}
		

		
		#endregion Constructors
		
		public override MTFiniteTimeAction Reverse()
		{
			throw new System.NotImplementedException ();
		}
		
		protected internal override MTActionState StartAction(GameObject target)
		{
			return new MTScaleToParticleState (this, target);
		}
	}
	
	public class MTScaleToParticleState : MTFiniteTimeActionState
	{
		protected float Delta;

		
		protected float EndScale;

		
		protected float StartScale;

		protected float OriginSize;

		
		public MTScaleToParticleState (MTScaleParticleTo action, GameObject target)
			: base (action, target)
		{ 
			StartScale = action.StartScale; // target.transform.localScale.x;

			OriginSize = target.GetParticleStartSize();
			
			EndScale = action.EndScale;

			
			Delta = EndScale - StartScale;

		}
		
		public override void Update (float time)
		{
			if (Target != null)
			{
				var Scale = StartScale + Delta * time;
				Target.SetParticleSize(Scale * OriginSize);
			}
		}
	}
}