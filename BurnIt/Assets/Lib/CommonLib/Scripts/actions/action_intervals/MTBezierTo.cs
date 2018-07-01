using UnityEngine;

namespace MTUnity.Actions
{
    public class MTBezierTo : MTBezierBy
    {
        #region Constructors

        public MTBezierTo (float t, MTBezierConfig c)
            : base (t, c)
        {
        }

        #endregion Constructors


        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTBezierToState (this, target);

        }

    }

    public class MTBezierToState : MTBezierByState
    {

        public MTBezierToState (MTBezierBy action, GameObject target)
            : base (action, target)
        { 
            var config = BezierConfig;

            config.ControlPoint1 -= StartPosition;
            config.ControlPoint2 -= StartPosition;
            config.EndPosition -= StartPosition;

            BezierConfig = config;
        }

    }

}