using System;
using System.Collections.Generic;

namespace GuiLabs.Undo
{
    /// <summary>
    /// Action Manager is a central class for the Undo Framework.
    /// Your domain model (business objects) will have an ActionManager reference that would 
    /// take care of executing actions.
    /// 
    /// Here's how it works:
    /// 1. You declare a class that implements IAction
    /// 2. You create an instance of it and give it all necessary info that it needs to know
    ///    to apply or rollback a change
    /// 3. You call ActionManager.RecordAction(yourAction)
    /// 
    /// Then you can also call ActionManager.Undo() or ActionManager.Redo()
    /// </summary>
    public class ActionManager
    {
        public ActionManager()
        {
            History = new SimpleHistory();
        }

        #region Events

        /// <summary>
        /// Listen to this event to be notified when a new action is added, executed, undone or redone
        /// </summary>
        public event EventHandler CollectionChanged;
        protected void RaiseUndoBufferChanged(object sender, EventArgs e)
        {
            if (CollectionChanged != null)
            {
                CollectionChanged(this, e);
            }
        }

        #endregion

        #region RecordAction

        #region Running

        /// <summary>
        /// Currently running action (during an Undo or Redo process)
        /// </summary>
        /// <remarks>null if no Undo or Redo is taking place</remarks>
        public IAction CurrentAction { get; internal set; }

        /// <summary>
        /// Checks if we're inside an Undo or Redo operation
        /// </summary>
        public bool ActionIsExecuting
        {
            get
            {
                return CurrentAction != null;
            }
        }

        #endregion

        /// <summary>
        /// Defines whether we should record an action to the Undo buffer and then execute,
        /// or just execute it without it becoming a part of history
        /// </summary>
        public bool ExecuteImmediatelyWithoutRecording { get; set; }

        /// <summary>
        /// Central method to add and execute a new action.
        /// </summary>
        /// <param name="existingAction">An action to be recorded in the buffer and executed</param>
        public void RecordAction(IAction existingAction)
        {
            if (existingAction == null)
            {
                throw new ArgumentNullException(
                    "ActionManager.RecordAction: the existingAction argument is null");
            }
            // make sure we're not inside an Undo or Redo operation
            CheckNotRunningBeforeRecording(existingAction);

            // if we don't want to record actions, just run and forget it
            if (ExecuteImmediatelyWithoutRecording
                && existingAction.CanExecute())
            {
                existingAction.Execute();
                return;
            }

            // Check if we're inside a transaction that is being recorded
            ITransaction currentTransaction = RecordingTransaction;
            if (currentTransaction != null)
            {
                // if we're inside a transaction, just add the action to the transaction's list
                currentTransaction.AccumulatingAction.Add(existingAction);
                if (!currentTransaction.IsDelayed)
                {
                    existingAction.Execute();
                }
            }
            else
            {
                RunActionDirectly(existingAction);
            }
        }

        void CheckNotRunningBeforeRecording(IAction existingAction)
        {
            string existing = existingAction != null ? existingAction.ToString() : "";

            if (CurrentAction != null)
            {
                throw new InvalidOperationException
                (
                    string.Format
                    (
                          "ActionManager.RecordActionDirectly: the ActionManager is currently running "
                        + "or undoing an action ({0}), and this action (while being executed) attempted "
                        + "to recursively record another action ({1}), which is not allowed. "
                        + "You can examine the stack trace of this exception to see what the "
                        + "executing action did wrong and change this action not to influence the "
                        + "Undo stack during its execution. Checking if ActionManager.ActionIsExecuting == true "
                        + "before launching another transaction might help to avoid the problem. Thanks and sorry for the inconvenience.",
                        CurrentAction.ToString(),
                        existing
                    )
                );
            }
        }

        object recordActionLock = new object();
        /// <summary>
        /// Adds the action to the buffer and runs it
        /// </summary>
        void RunActionDirectly(IAction actionToRun)
        {
            CheckNotRunningBeforeRecording(actionToRun);

            lock (recordActionLock)
            {
                CurrentAction = actionToRun;
                if (History.AppendAction(actionToRun))
                {
                    History.MoveForward();
                }
                CurrentAction = null;
            }
        }

        #endregion

        #region Transactions

        public Transaction CreateTransaction()
        {
            return Transaction.Create(this);
        }

        public Transaction CreateTransaction(bool delayed)
        {
            return Transaction.Create(this, delayed);
        }

        private Stack<ITransaction> mTransactionStack = new Stack<ITransaction>();
        public Stack<ITransaction> TransactionStack
        {
            get
            {
                return mTransactionStack;
            }
            set
            {
                mTransactionStack = value;
            }
        }

        public ITransaction RecordingTransaction
        {
            get
            {
                if (TransactionStack.Count > 0)
                {
                    return TransactionStack.Peek();
                }
                return null;
            }
        }

        public void OpenTransaction(ITransaction t)
        {
            TransactionStack.Push(t);
        }

        public void CommitTransaction()
        {
            if (TransactionStack.Count == 0)
            {
                throw new InvalidOperationException(
                    "ActionManager.CommitTransaction was called"
                    + " when there is no open transaction (TransactionStack is empty)."
                    + " Please examine the stack trace of this exception to find code"
                    + " which called CommitTransaction one time too many."
                    + " Normally you don't call OpenTransaction and CommitTransaction directly,"
                    + " but use using(var t = Transaction.Create(Root)) instead.");
            }

            ITransaction committing = TransactionStack.Pop();

            if (committing.AccumulatingAction.Count > 0)
            {
                RecordAction(committing.AccumulatingAction);
            }
        }

        public void RollBackTransaction()
        {
            if (TransactionStack.Count != 0)
            {
                var topLevelTransaction = TransactionStack.Peek();
                if (topLevelTransaction != null && topLevelTransaction.AccumulatingAction != null)
                {
                    topLevelTransaction.AccumulatingAction.UnExecute();
                }

                TransactionStack.Clear();
            }
        }

        #endregion

        #region Undo, Redo

        public void Undo()
        {
            if (!CanUndo)
            {
                return;
            }
            if (ActionIsExecuting)
            {
                throw new InvalidOperationException(string.Format("ActionManager is currently busy"
                    + " executing a transaction ({0}). This transaction has called Undo()"
                    + " which is not allowed until the transaction ends."
                    + " Please examine the stack trace of this exception to see"
                    + " what part of your code called Undo.", CurrentAction));
            }
            CurrentAction = History.CurrentState.PreviousAction;
            History.MoveBack();
            CurrentAction = null;
        }

        public void Redo()
        {
            if (!CanRedo)
            {
                return;
            }
            if (ActionIsExecuting)
            {
                throw new InvalidOperationException(string.Format("ActionManager is currently busy"
                    + " executing a transaction ({0}). This transaction has called Redo()"
                    + " which is not allowed until the transaction ends."
                    + " Please examine the stack trace of this exception to see"
                    + " what part of your code called Redo.", CurrentAction));
            }
            CurrentAction = History.CurrentState.NextAction;
            History.MoveForward();
            CurrentAction = null;
        }

        public bool CanUndo
        {
            get
            {
                return History.CanMoveBack;
            }
        }

        public bool CanRedo
        {
            get
            {
                return History.CanMoveForward;
            }
        }

        #endregion

        #region Buffer

        public void Clear()
        {
            History.Clear();
            CurrentAction = null;
        }

        public IEnumerable<IAction> EnumUndoableActions()
        {
            return History.EnumUndoableActions();
        }

        private IActionHistory mHistory;
        internal IActionHistory History
        {
            get
            {
                return mHistory;
            }
            set
            {
                if (mHistory != null)
                {
                    mHistory.CollectionChanged -= RaiseUndoBufferChanged;
                }
                mHistory = value;
                if (mHistory != null)
                {
                    mHistory.CollectionChanged += RaiseUndoBufferChanged;
                }
            }
        }

        #endregion
    }
}
