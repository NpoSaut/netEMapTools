using System.Collections.Generic;

namespace MapViewer.Environment
{
    public interface IStartupArgumentsProvider
    {
        IList<string> Arguments { get; }
    }
}
