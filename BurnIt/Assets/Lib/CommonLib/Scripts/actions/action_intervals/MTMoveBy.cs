using UnityEngine;

namespace MTUnity.Actions
{
    public class MTMoveBy : MTFiniteTimeAction
    {
        #region Constructors

        public MTMoveBy (float duration, Vector3 position) : base (duration)
        {
            PositionDelta = position;
        }

        #endregion Constructors

        public Vector3 PositionDelta { get; private set; }

        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTMoveByState (this, target);
        }

        public override MTFiniteTimeAction Reverse ()
        {
			return new MTMoveBy (Duration, new Vector3 (-PositionDelta.x, -PositionDelta.y,-PositionDelta.z));
        }
    }

    public class MTMoveByState : MTFiniteTimeActionState
    {
        protected Vector3 PositionDelta;
        protected Vector3 EndPosition;
        protected Vector3 StartPosition;
		public Vector3 PreviousPosition {
			get;
			protected set;
		}

        public MTMoveByState (MTMoveBy action, GameObject target)
            : base (action, target)
        { 
			PositionDelta = action.PositionDelta;
			if(target == null)
			{
				return;
			}
			PreviousPosition = StartPosition = target.transform.localPosition;
        }

        public override void Update (float time)
        {
            if (Target == null)
                return;

			var currentPos = Target.transform.localPosition;
            var diff = currentPos - PreviousPosition;
            StartPosition = StartPosition + diff;
			Vector3 newPos = StartPosition + PositionDelta * time;
			PreviousPosition = newPos;
			Target.transform.localPosition = newPos;
        }
    }

}