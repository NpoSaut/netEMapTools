using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using InstallerTools;
using WixSharp;
using WixSharp.CommonTasks;
using Assembly = System.Reflection.Assembly;

public class Script
{
    public static void Main(string[] args)
    {
        var dir = new DirectoryInfo(args.Length > 0 ? args[0] : @"..\..\..\EMapNavigator\bin\Release");
        var outputDir = new DirectoryInfo(args.Length > 1 ? args[1] : @"..\..\..\installers");

        string exeFileFullName = dir.EnumerateFiles("*.exe").First().FullName;
        string exeFileName = Path.GetFileName(exeFileFullName);
        Assembly assembly = Assembly.LoadFile(exeFileFullName);

        string projectName = assembly.GetCustomAttributes<AssemblyTitleAttribute>().First().Title;

        var mainFeature = new Feature("Программа", true, false) { ConfigurableDir = "INSTALLDIR" };

        var project = new Project(projectName,
                                  new Dir(mainFeature, InstallerHelper.GetSautProgramLocation(projectName),
                                          new Files(dir.FullName + "\\*.*", InstallerHelper.IsFileDesired)),

                                  // Shortcuts
                                  new Dir(mainFeature, @"%ProgramMenu%",
                                          new ExeFileShortcut(projectName, "[INSTALLDIR]" + exeFileName, "") { WorkingDirectory = "INSTALLDIR" }
                                      ),
                                  new Dir(mainFeature, "%Desktop%",
                                          new ExeFileShortcut(mainFeature, projectName, "[INSTALLDIR]" + exeFileName, "") { WorkingDirectory = "INSTALLDIR" }
                                      ))
                      {
                          DefaultFeature = mainFeature,
                          OutDir = outputDir.FullName
                      };
        
        project.ResolveWildCards(true)
               .FindFile(f => f.Name.EndsWith(exeFileName))
               .First()
               .AddAssociation(new FileAssociation("gps", "application/blok-map-project", "Открыть в MapViewer", "\"%1\"")
                               {
                                   Icon = "gpsfile.ico",
                                   Description = "Карта КЛУБ/БЛОК"
                               });

        project.SetBasicThings(new Guid("E9D8C57F-D4D0-4772-A508-1F21C627B892"), DotNetVersion.DotNet45);
        project.SetInterface(InstallerInterfaceKind.SelectDirectory);
        project.SetProjectInformation(assembly,
                                      Path.Combine(dir.FullName, "..", "..", "icon.ico"),
                                      "https://repo.saut.ru/#/tools?tool=MapViewer");

        string msi = Compiler.BuildMsi(project);
        InstallerHelper.SignInstaller(msi);
    }
}
