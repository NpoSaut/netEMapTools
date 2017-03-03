using System.Collections.Generic;

namespace MapViewer.Environment.Implementations
{
    public class ListStartupArgumentsProvider : IStartupArgumentsProvider
    {
        public ListStartupArgumentsProvider(IList<string> Arguments) { this.Arguments = Arguments; }
        public IList<string> Arguments { get; private set; }
    }
}