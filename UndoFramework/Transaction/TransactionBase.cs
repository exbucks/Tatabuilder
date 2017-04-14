namespace GuiLabs.Undo
{
    public class TransactionBase : ITransaction
    {
        #region ctors

        public TransactionBase(ActionManager am, bool isDelayed)
            : this(am)
        {
            IsDelayed = isDelayed;
        }

        public TransactionBase(ActionManager am)
            : this()
        {
            ActionManager = am;
            if (am != null)
            {
                am.OpenTransaction(this);
            }
        }

        public TransactionBase()
        {
            IsDelayed = true;
        }

        #endregion

        public ActionManager ActionManager { get; private set; }
        public bool IsDelayed { get; set; }

        protected IMultiAction accumulatingAction;
        public IMultiAction AccumulatingAction
        {
            get
            {
                return accumulatingAction;
            }
        }

        public virtual void Commit()
        {
            if (ActionManager != null)
            {
                ActionManager.CommitTransaction();
            }
        }

        public virtual void Rollback()
        {
            if (ActionManager != null)
            {
                ActionManager.RollBackTransaction();
                Aborted = true;
            }
        }

        public bool Aborted { get; set; }

        public virtual void Dispose()
        {
            if (!Aborted)
            {
                Commit();
            }
        }
    }
}
