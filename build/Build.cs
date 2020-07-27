using System.IO;
using CommandLine;
using InstallerTools;

class Build : BuilderBase
{
    public static void Main(string[] Args)
    {
        var builder = new Build();

        Parser.Default.ParseArguments<Parameters>(Args)
              .WithParsed(o =>
               {
                   builder.Run("EMapNavigator",
                               Configuration: o.Configuration,
                               PublishPlace: o.PublishPlace);
               });
    }

    protected override IInstallerScript CreateInstallerScript(
        string ProjectDirectory, string BinariesOutput, string TargetFramework) =>
        new MapViewerInstallerScript(BinariesOutput, Path.Combine(ProjectDirectory, "icon.ico"), TargetFramework);
}