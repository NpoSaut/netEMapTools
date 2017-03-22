using System;

namespace MapViewer
{
    public interface IWindowTitleManager
    {
        IDisposable PutText(int Order, string Text);
        string Title { get; }
    }
}
