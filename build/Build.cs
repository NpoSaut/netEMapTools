using InstallerTools;

class Build : BuilderBase
{
    public static int Main()
    {
        return Execute<Build>(x => x.Compile);
    }

    protected override IInstallerScript CreateInstallerScript()
    {
        return new MapViewerInstallerScript(ProjectPath / "bin" / Configuration,
                                            SolutionDirectory / "!res" / "icon" / "icon.ico");
    }
}
