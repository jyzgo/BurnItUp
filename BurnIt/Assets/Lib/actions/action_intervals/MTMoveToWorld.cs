using UnityEngine;

namespace MTUnity.Actions
{
    public class MTMoveToWorld : MTMoveBy
    {
        protected Vector3 EndPosition;

        #region Constructors

        public MTMoveToWorld(float duration, Vector3 position) : base(duration, position)
        {
            EndPosition = position;
        }
        public static MTFiniteTimeAction Create(float duration, MonoBehaviour curMono)
        {
            if (curMono == null || curMono.gameObject == null)
                return null;
            var curTaget = curMono.gameObject.transform.position;

            var curMove = new MTMoveToWorld(duration, curTaget);
            return curMove;
        }

        #endregion Constructors

        public Vector3 PositionEnd
        {
            get { return EndPosition; }
        }

        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTMoveToWorldState(this, target);

        }
    }

    public class MTMoveToWorldState : MTMoveByState
    {

        public MTMoveToWorldState(MTMoveToWorld action, GameObject target)
            : base(action, target)
        {
            StartPosition = target.transform.position;
            PositionDelta = action.PositionEnd - target.transform.position;
            endPos = action.PositionEnd;
        }

        Vector3 endPos;

        public override void Update(float time)
        {
            if (Target != null)
            {
                Vector3 newPos = StartPosition + PositionDelta * time;
                Target.transform.position = newPos;
                PreviousPosition = newPos;
            }
        }

        protected internal override void Stop()
        {
            Target.transform.position = endPos;
            base.Stop();
           
        }

    }

}