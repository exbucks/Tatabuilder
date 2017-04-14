using System.Collections.Generic;

namespace GuiLabs.Undo
{
    public interface IMultiAction : IAction, IList<IAction>
    {
        bool IsDelayed { get; set; }
    }
}
