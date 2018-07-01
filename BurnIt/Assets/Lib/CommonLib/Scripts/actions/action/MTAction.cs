using UnityEngine;

namespace MTUnity.Actions
{
    public enum MTActionTag
    {
        //! Default tag
        Invalid = -1,
    }

    public abstract class MTAction
    {
        public int Tag { get; set; }


        #region Constructors

        protected MTAction()
        {
            Tag = (int)MTActionTag.Invalid;
        }

        #endregion Constructor


        protected internal virtual MTActionState StartAction (GameObject target)
        {
            return null;

        }
    }

    public abstract class MTActionState
    {
        /// <summary>
        /// Gets or sets the target.
        /// 
        /// Will be set with the 'StartAction' method of the corresponding Action. 
        /// When the 'Stop' method is called, Target will be set to null. 
        /// 
        /// </summary>
        /// <value>The target.</value>

        #region Properties

        public GameObject Target { get; protected set; }
        public GameObject OriginalTarget { get; protected set; }
        public MTAction Action { get; protected set; }

//        protected MTScene Scene { get; private set; }
//        protected MTLayer Layer { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is done.
        /// </summary>
        /// <value><c>true</c> if this instance is done; otherwise, <c>false</c>.</value>
        public virtual bool IsDone 
        {
            get { return true; }
        }

        #endregion Properties


        public MTActionState (MTAction action, GameObject target)
        {
            this.Action = action;
            this.Target = target;
            this.OriginalTarget = target;
//            if (target != null)
//                this.Layer = target.Layer;
//				Debug.Assert(false,"not implement");

        }

        /// <summary>
        /// Called after the action has finished.
        /// It will set the 'Target' to null. 
        /// IMPORTANT: You should never call this method manually. Instead, use: "target.StopAction(actionState);"
        /// </summary>
        protected internal virtual void Stop()
        {
            Target = null;
        }

        /// <summary>
        /// Called every frame with it's delta time. 
        /// 
        /// DON'T override unless you know what you are doing.
        /// 
        /// </summary>
        /// <param name="dt">Delta Time</param>
        protected internal virtual void Step (float dt)
        {
            #if DEBUG
           Debug.Log ("[Action State step]. override me");
            #endif
        }

        /// <summary>
        /// Called once per frame.
        /// </summary>
        /// <param name="time">A value between 0 and 1
        ///
        /// For example:
        ///
        /// 0 means that the action just started
        /// 0.5 means that the action is in the middle
        /// 1 means that the action is over</param>
        public virtual void Update (float time)
        {
            #if DEBUG
           Debug.Log ("[Action State update]. override me");
            #endif
        }
    }
}