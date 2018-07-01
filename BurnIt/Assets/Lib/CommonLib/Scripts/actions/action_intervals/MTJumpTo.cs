using UnityEngine;

namespace MTUnity.Actions
{
    public class MTJumpTo : MTJumpBy
    {
        #region Constructors

        public MTJumpTo (float duration, Vector3 position, float height, uint jumps) 
            : base (duration, position, height, jumps)
        {
        }

        #endregion Constructors

        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTJumpToState (this, target);

        }

    }

    public class MTJumpToState : MTJumpByState
    {

        public MTJumpToState (MTJumpBy action, GameObject target)
            : base (action, target)
        { 
			Delta = new Vector3 (Delta.x - StartPosition.x, Delta.y - StartPosition.y,Delta.z - StartPosition.z);
        }
    }

}