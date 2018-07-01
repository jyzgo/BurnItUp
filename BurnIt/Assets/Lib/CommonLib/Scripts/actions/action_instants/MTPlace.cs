using UnityEngine;

namespace MTUnity.Actions
{
    public class MTPlace : MTActionInstant
    {
        public Vector3 Position { get; private set; }


        #region Constructors

		bool _isWorld = false;
		public MTPlace (Vector3 pos,bool isWorld = false)
        {
            Position = pos;
			_isWorld = isWorld;
        }

        public MTPlace (int posX, int posY , int posZ)
        {
            Position = new Vector3 (posX, posY,posZ);
        }

        #endregion Constructors

        protected internal override MTActionState StartAction(GameObject target)
        {
			return new MTPlaceState (this, target,_isWorld);

        }
    }

    public class MTPlaceState : MTActionInstantState
    {

		public MTPlaceState (MTPlace action, GameObject target,bool isWorld)
            : base (action, target)
        { 
			if(isWorld)
			{
				Target.transform.position = action.Position;
			}else
			{
				Target.transform.localPosition = action.Position;
			}
        }

    }

}