using System;

using UnityEngine;

namespace MTUnity.Actions
{
    public class MTRotateToTest : MTFiniteTimeAction
    {

        #region Constructors

        public new float Duration { get; private set; }
        public Vector3 TargetAngle { get; private set; }

        public MTRotateToTest(float duration, Vector3 toAngle) : base(duration)
        {
            Duration = duration;
            TargetAngle = toAngle;
        }

        #endregion Constructors

        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTRotateToStateTest(this, target);
        }

        public override MTFiniteTimeAction Reverse()
        {
            throw new NotImplementedException();
        }
    }


    public class MTRotateToStateTest : MTFiniteTimeActionState
    {


        public MTRotateToStateTest(MTRotateToTest action, GameObject target)
            : base(action, target)
        {
            if (Target == null)
            {
                return;
            }
            FromAngle = Target.transform.localRotation.eulerAngles;
            ToAngle = action.TargetAngle;// Quaternion.Euler( action.TargetAngle);
            InTime = action.Duration;
        }

        Vector3 FromAngle;
        Vector3 ToAngle;
        float InTime;
        float curTime = 0f;

        public override void Update(float time)
        {

            if (Target != null)
            {
                float curTimeScale = curTime / InTime;
                float x = Mathf.Lerp( FromAngle.x, ToAngle.x,curTimeScale);
                float y = Mathf.Lerp( FromAngle.y, ToAngle.y,curTimeScale);
                float z = Mathf.Lerp(FromAngle.z,ToAngle.z  ,curTimeScale);
                var newV = new Vector3(x, y, z);

                Target.transform.localRotation = Quaternion.Euler(newV);
            }

            curTime += Time.deltaTime;

        }

    }
}