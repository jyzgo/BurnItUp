using System;

using UnityEngine;

namespace MTUnity.Actions
{
    public class MTRotateTo : MTFiniteTimeAction
    {

        #region Constructors
	
		public new float Duration{get;private set;}
		//public Vector3 TargetAngle{get;private set;}
        public Quaternion TargetQuaternion { get; private set; }

		public MTRotateTo(float duration,Vector3 toAngle):base(duration)
		{
			Duration = duration;
			TargetQuaternion =Quaternion.Euler( toAngle);
		}

        public MTRotateTo(float duration,Quaternion toQuater):base(duration)
        {
            Duration = duration;
            TargetQuaternion = toQuater;
        }

        #endregion Constructors

        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTRotateToState (this, target);
        }

        public override MTFiniteTimeAction Reverse()
        {
            throw new NotImplementedException();
        }
    }


    public class MTRotateToState : MTFiniteTimeActionState
    {


        public MTRotateToState (MTRotateTo action, GameObject target)
            : base (action, target)
        { 
			if(Target == null)
			{
				return;
			}
			FromAngle = Target.transform.localRotation;
            ToAngle = action.TargetQuaternion;// Quaternion.Euler( action.TargetAngle);
			InTime = action.Duration;
		}

		Quaternion FromAngle;
		Quaternion ToAngle;
		float InTime;
		float curTime = 0f;

        public override void Update (float time)
        {
			if(Target != null ){
				Target.transform.localRotation = Quaternion.Lerp(FromAngle,ToAngle,curTime/InTime);
			}

			curTime += Time.deltaTime;

        }

    }
}