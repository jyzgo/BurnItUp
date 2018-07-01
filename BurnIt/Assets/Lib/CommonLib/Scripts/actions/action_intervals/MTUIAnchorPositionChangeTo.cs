using UnityEngine;

namespace MTUnity.Actions
{
    public class MTUIAnchorPositionChangeTo : MTFiniteTimeAction
    {
        protected Vector2 EndPosition;

        #region Constructors
        public MTUIAnchorPositionChangeTo (float duration, Vector2 position) : base (duration)
        {
            EndPosition = position;
        }
        #endregion Constructors

        public Vector2 PositionEnd {
            get { return EndPosition; }
        }

        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTUIAnchorPositionChangeToState (this, target);
        }

        public override MTFiniteTimeAction Reverse(){
            Debug.LogError("not support");
            //return new MTUISizeChangeTo(Duration, new Vector2(- );
            return null;
        }
    }

    public class MTUIAnchorPositionChangeToState : MTFiniteTimeActionState
	{
        RectTransform trans;
        protected Vector2 PositionDelta;
        protected Vector2 EndPosition;
        protected Vector2 StartPosition;
        public Vector2 PreviousPosition {
            get;
            protected set;
        }

        public MTUIAnchorPositionChangeToState (MTUIAnchorPositionChangeTo action, GameObject target)
            : base (action, target)
        { 
            if(target == null)
            {
                return;
            }
            trans = target.GetComponent<RectTransform>();
            var targetUICurPosition = trans.anchoredPosition;
            PositionDelta = action.PositionEnd - targetUICurPosition;
            PreviousPosition = StartPosition = targetUICurPosition;
        }

        public override void Update (float time)
        {
            if (Target != null)
            {
                Vector2 newSize = StartPosition + PositionDelta * time;
                PreviousPosition = newSize;
                trans.anchoredPosition = newSize;
            }
        }

    }

}