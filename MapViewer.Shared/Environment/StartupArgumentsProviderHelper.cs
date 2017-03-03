using System.IO;
using System.Linq;
using MapVisualization.Annotations;

namespace MapViewer.Environment
{
    public static class StartupArgumentsProviderHelper
    {
        [CanBeNull]
        public static string FindFilePath([NotNull] this IStartupArgumentsProvider ArgumentsProvider, [NotNull] string FileExtension)
        {
            char[] invalidCharacters = Path.GetInvalidPathChars();
            return ArgumentsProvider.Arguments
                                    .Select(a => a.Trim('\"'))
                                    .FirstOrDefault(a => !invalidCharacters.Any(a.Contains) &&
                                                         Path.GetExtension(a) == "." + FileExtension);
        }
    }
}
