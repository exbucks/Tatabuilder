using System;

namespace GuiLabs.Undo
{
    public interface ITransaction : IDisposable
    {
        IMultiAction AccumulatingAction { get; }
        bool IsDelayed { get; set; }
    }
}
