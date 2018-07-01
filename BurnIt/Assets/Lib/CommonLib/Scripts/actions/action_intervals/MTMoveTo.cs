using UnityEngine;

namespace MTUnity.Actions
{
    public class MTMoveTo : MTMoveBy
    {
        protected Vector3 EndPosition;

		bool _isWorld = false;
        #region Constructors

		public MTMoveTo (float duration, Vector3 position,bool isWorld = false) : base (duration, position)
        {
            EndPosition = position;
			_isWorld = isWorld;
        }

        #endregion Constructors

        public Vector3 PositionEnd {
            get { return EndPosition; }
        }

        protected internal override MTActionState StartAction(GameObject target)
        {
			return new MTMoveToState (this, target,_isWorld);

        }
    }

    public class MTMoveToState : MTMoveByState
	{
		public bool IsWorld {
			get;
			protected set;
		}

		public MTMoveToState (MTMoveTo action, GameObject target,bool isWorld)
            : base (action, target)
        { 
			if(target == null)
			{
				return;
			}
			IsWorld = isWorld;
			if(isWorld)
			{
				StartPosition = target.transform.position;
				PositionDelta = action.PositionEnd - target.transform.position;
			}else
			{
				StartPosition = target.transform.localPosition;
				PositionDelta = action.PositionEnd - target.transform.localPosition;
			}
        }

        public override void Update (float time)
        {
            if (Target != null)
            {
				Vector3 newPos = StartPosition + PositionDelta * time;
				PreviousPosition = newPos;
				if(IsWorld)
				{
					Target.transform.position = newPos;
				}else
				{
					Target.transform.localPosition = newPos;
				}
            }
        }
    }

}