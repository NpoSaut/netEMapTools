using System.Windows.Input;

namespace MapViewer.InterfaceElements
{
    public interface IToolbarCommand
    {
        string Name { get; }
        string Description { get; }
        ICommand Command { get; }
    }
}
