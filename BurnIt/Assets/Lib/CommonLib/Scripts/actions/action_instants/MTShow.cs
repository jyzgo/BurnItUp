using UnityEngine;

namespace MTUnity.Actions
{
    public class MTShow : MTActionInstant
    {
        #region Constructors

        public MTShow ()
        {
        }

        #endregion Constructors

        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTShowState (this, target);

        }

        public override MTFiniteTimeAction Reverse ()
        {
            return (new MTHide ());
        }

    }

    public class MTShowState : MTActionInstantState
    {

        public MTShowState (MTShow action, GameObject target)
            : base (action, target)
        {   
			var render = target.GetComponent<Renderer> ();
			if (render) {
				render.enabled = true;
			}
        }

    }

}