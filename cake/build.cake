//#addin Cake.MSBuildTask

using System;
using System.IO;

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var srcDir = Directory("..");
var buildDir = Directory("./binin");
var solution = System.IO.Path.Combine(srcDir, "EMapTools.sln");

Task("Clean")
    .Does(() =>
{
    CleanDirectory(buildDir);
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore(solution);
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    // Use MSBuild
    MSBuild(solution, settings =>
        settings
            .SetConfiguration(configuration)
			.WithTarget("Rebuild")
			.SetVerbosity(Verbosity.Minimal));
});

Task("MakeInstaller")
    .IsDependentOn("Build")
    .Does(() =>
{
    Information("Build: " + System.IO.Path.Combine(srcDir, "Installer", "bin", configuration, "Installer.exe"));
    IEnumerable<string> stdout = new List<string> ();
    StartProcess (System.IO.Path.Combine(srcDir, "Installer", "bin", configuration, "Installer.exe"),
        new ProcessSettings {
            WorkingDirectory = System.IO.Path.Combine(srcDir, "Installer", "bin", configuration),
            RedirectStandardOutput = true,
        }, out stdout);
    var output = stdout.ToList();
    if (!output.Contains("the MSI file was signed successfully."))
    {
        foreach(var l in output)
        {
            Information(">> " + l);
        }
        throw new Exception("Не удалось собрать инсталлятор");
    };
});

Task("Default")
    .IsDependentOn("MakeInstaller");

RunTarget(target);
