using System;

using UnityEngine;

namespace MTUnity.Actions
{
    public class MTBezierBy : MTFiniteTimeAction
    {
        public MTBezierConfig BezierConfig { get; private set; }


        #region Constructors

        public MTBezierBy (float t, MTBezierConfig config) : base (t)
        {
            BezierConfig = config;
        }

        #endregion Constructors


        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTBezierByState (this, target);

        }

        public override MTFiniteTimeAction Reverse ()
        {
            MTBezierConfig r;

            r.EndPosition = -BezierConfig.EndPosition;
            r.ControlPoint1 = BezierConfig.ControlPoint2 + -BezierConfig.EndPosition;
            r.ControlPoint2 = BezierConfig.ControlPoint1 + -BezierConfig.EndPosition;

            var action = new MTBezierBy (Duration, r);
            return action;
        }
    }

    public class MTBezierByState : MTFiniteTimeActionState
    {
        protected MTBezierConfig BezierConfig { get; set; }

        protected Vector3 StartPosition { get; set; }

        protected Vector3 PreviousPosition { get; set; }


        public MTBezierByState (MTBezierBy action, GameObject target)
            : base (action, target)
        { 
            BezierConfig = action.BezierConfig;
			PreviousPosition = StartPosition = target.transform.localPosition;
        }

        public override void Update (float time)
        {
            if (Target != null)
            {
                float xa = 0;
                float xb = BezierConfig.ControlPoint1.x;
                float xc = BezierConfig.ControlPoint2.x;
                float xd = BezierConfig.EndPosition.x;

                float ya = 0;
                float yb = BezierConfig.ControlPoint1.y;
                float yc = BezierConfig.ControlPoint2.y;
                float yd = BezierConfig.EndPosition.y;

                float za = 0;
                float zb = BezierConfig.ControlPoint1.z;
                float zc = BezierConfig.ControlPoint2.z;
                float zd = BezierConfig.EndPosition.z;

                float x = MTSplineMath.CubicBezier (xa, xb, xc, xd, time);
                float y = MTSplineMath.CubicBezier (ya, yb, yc, yd, time);
                float z = MTSplineMath.CubicBezier (za, zb, zc, zd, time);

				Vector3 currentPos = Target.transform.localPosition;
                Vector3 diff = currentPos - PreviousPosition;
                StartPosition = StartPosition + diff;

                Vector3 newPos = StartPosition + new Vector3 (x, y,z);
				Target.transform.localPosition = newPos;

                PreviousPosition = newPos;
            }
        }

    }

    public struct MTBezierConfig
    {
        public Vector3 ControlPoint1;
        public Vector3 ControlPoint2;
        public Vector3 EndPosition;
    }
}