namespace GuiLabs.Undo
{
    public class Transaction : TransactionBase
    {
        protected Transaction(ActionManager actionManager, bool delayed)
            : base(actionManager, delayed)
        {
            this.accumulatingAction = new MultiAction();
        }

        public static Transaction Create(ActionManager actionManager, bool delayed)
        {
            return new Transaction(actionManager, delayed);
        }

        /// <summary>
        /// By default, the actions are delayed and executed only after
        /// the top-level transaction commits.
        /// </summary>
        /// <remarks>
        /// Make sure to dispose of the transaction once you're done - it will actually call Commit for you
        /// </remarks>
        public static Transaction Create(ActionManager actionManager)
        {
            return Create(actionManager, true);
        }

        public override void Commit()
        {
            this.AccumulatingAction.IsDelayed = this.IsDelayed;
            base.Commit();
        }
    }
}
