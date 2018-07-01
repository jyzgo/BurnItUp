using UnityEngine;

namespace MTUnity.Actions
{
    public class MTToggleVisibility : MTActionInstant
    {
        #region Constructors

        public MTToggleVisibility ()
        {
        }

        #endregion Constructors


        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTToggleVisibilityState (this, target);

        }
    }

    public class MTToggleVisibilityState : MTActionInstantState
    {

        public MTToggleVisibilityState (MTToggleVisibility action, GameObject target)
            : base (action, target)
        {   
			var render = target.GetComponent<Renderer> ();
			if (render) {
				render.enabled = !render.enabled;
			}
        }

    }

}