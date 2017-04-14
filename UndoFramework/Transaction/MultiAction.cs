using System.Collections.Generic;

namespace GuiLabs.Undo
{
    public class MultiAction : List<IAction>, IMultiAction
    {
        public MultiAction()
        {
            IsDelayed = true;
        }

        public bool IsDelayed { get; set; }

        public void Execute()
        {
            if (!IsDelayed)
            {
                IsDelayed = true;
                return;
            }
            foreach (var action in this)
            {
                action.Execute();
            }
        }

        public void UnExecute()
        {
            var reversed = new List<IAction>(this);
            reversed.Reverse();
            foreach (var action in reversed)
            {
                action.UnExecute();
            }
        }

        public bool CanExecute()
        {
            foreach (var action in this)
            {
                if (!action.CanExecute())
                {
                    return false;
                }
            }
            return true;
        }

        public bool CanUnExecute()
        {
            foreach (var action in this)
            {
                if (!action.CanUnExecute())
                {
                    return false;
                }
            }
            return true;
        }

        public bool TryToMerge(IAction FollowingAction)
        {
            return false;
        }

        public bool AllowToMergeWithPrevious { get; set; }
    }
}
