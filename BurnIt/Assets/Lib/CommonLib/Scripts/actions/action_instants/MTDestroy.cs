using System;
using System.Collections.Generic;

using UnityEngine;

namespace MTUnity.Actions
{
	public class MTDestroy : MTActionInstant
	{

		
		#region Constructors
		

		float _delayTime = 0f;
		public MTDestroy (float delayTime = 0f)
		{
			_delayTime = delayTime;
		}
		
		#endregion Constructors
		
		
		protected internal override MTActionState StartAction(GameObject target)
		{
			return new MTDestroyState (this, target,_delayTime);
			
		}
		
		
		public override MTFiniteTimeAction Reverse ()
		{
			return new MTDestroy (_delayTime);
		}
	}
	
	public class MTDestroyState : MTActionInstantState
	{
		float _delayTime = 0f;
		
		public MTDestroyState (MTDestroy action, GameObject target,float delayTime = 0f)
			: base (action, target)
		{   
			_delayTime = delayTime;
		}
		
		public override void Update (float time)
		{
			if (Target && Target.gameObject) 
			{
				ObjectPool.PoolDestroy(Target.gameObject,_delayTime);
			}
		}
		
	}
	
}