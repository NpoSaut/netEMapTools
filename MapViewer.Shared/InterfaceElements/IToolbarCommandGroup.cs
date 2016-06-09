using System.Collections.Generic;

namespace MapViewer.InterfaceElements
{
    public interface IToolbarCommandGroup
    {
        IList<IToolbarCommand> Commands { get; }
    }
}
