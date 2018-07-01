using System;
//using System.Diagnostics;

using UnityEngine;

namespace MTUnity.Actions
{
    // Extra action for making a MTSequence or MTSpawn when only adding one action to it.
    internal class MTExtraAction : MTFiniteTimeAction
    {
        public override MTFiniteTimeAction Reverse ()
        {
            return new MTExtraAction ();
        }

        protected internal override MTActionState StartAction(GameObject target)
        {
            return new MTExtraActionState (this, target);

        }

        #region Action State

        public class MTExtraActionState : MTFiniteTimeActionState
        {

            public MTExtraActionState (MTExtraAction action, GameObject target)
                : base (action, target)
            {
            }

            protected internal override void Step (float dt)
            {
            }

            public override void Update (float time)
            {
            }
        }

        #endregion Action State
    }
}