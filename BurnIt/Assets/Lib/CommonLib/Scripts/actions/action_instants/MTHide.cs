using UnityEngine;

namespace MTUnity.Actions
{
    public class MTHide : MTActionInstant
    {
        #region Constructors

        public MTHide ()
        {
        }

        #endregion Constructors


        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTHideState (this, target);

        }

        public override MTFiniteTimeAction Reverse ()
        {
            return (new MTShow ());
        }

    }

    public class MTHideState : MTActionInstantState
    {

        public MTHideState (MTHide action, GameObject target)
            : base (action, target)
        {   
			var render = target.GetComponent<Renderer> ();
			if (render) {
				var curColor =  render.material.color ;
				render.material.color = new Color(curColor.r,curColor.g,curColor.b,0f);
			}
        }

    }

}