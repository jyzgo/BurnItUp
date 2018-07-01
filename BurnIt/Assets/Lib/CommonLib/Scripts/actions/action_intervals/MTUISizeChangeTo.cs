using UnityEngine;

namespace MTUnity.Actions
{
    public class MTUISizeChangeTo : MTFiniteTimeAction
    {
        protected Vector2 Endsize;

        #region Constructors
        public MTUISizeChangeTo (float duration, Vector2 size) : base (duration)
        {
            Endsize = size;
        }
        #endregion Constructors

        public Vector2 SizeEnd {
            get { return Endsize; }
        }

        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTUISizeChangeToState (this, target);
        }

        public override MTFiniteTimeAction Reverse(){
            Debug.LogError("not support");
            //return new MTUISizeChangeTo(Duration, new Vector2(- );
            return null;
        }
    }

    public class MTUISizeChangeToState : MTFiniteTimeActionState
	{
        RectTransform trans;
        protected Vector2 SizeDelta;
        protected Vector2 EndSize;
        protected Vector2 StartSize;
        public Vector2 PreviousSize {
            get;
            protected set;
        }

        public MTUISizeChangeToState (MTUISizeChangeTo action, GameObject target)
            : base (action, target)
        { 
            if(target == null)
            {
                return;
            }
            trans = target.GetComponent<RectTransform>();
            var targetUICurSize = trans.sizeDelta;
            SizeDelta = action.SizeEnd - targetUICurSize;
            PreviousSize = StartSize = targetUICurSize;
        }

        public override void Update (float time)
        {
            if (Target != null)
            {
				Vector2 newSize = StartSize + SizeDelta * time;
                PreviousSize = newSize;
                trans.sizeDelta = newSize;
            }
        }

    }

}