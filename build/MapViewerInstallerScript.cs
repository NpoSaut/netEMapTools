﻿using System;
using System.Linq;
using InstallerTools;
using Nuke.Core.IO;
using WixSharp;
using WixSharp.CommonTasks;

class MapViewerInstallerScript : InstallerScript
{
    public MapViewerInstallerScript(PathConstruction.AbsolutePath BinariesPath, string IconFile) : base(BinariesPath)
    {
        this.IconFile = IconFile;
    }

    protected override string        IconFile        { get; }
    protected override Guid          ApplicationGuid => new Guid("E9D8C57F-D4D0-4772-A508-1F21C627B892");
    protected override DotNetVersion DotNetVersion   => DotNetVersion.DotNet45;
    protected override string        WebPageLink     => "https://repo.saut.ru/#/tools?tool=MapViewer";

    protected override void ConfigureProject(Project Project)
    {
        Project.ResolveWildCards(true)
               .FindFile(f => f.Name.EndsWith(ExeFileName))
               .First()
               .AddAssociation(new FileAssociation("gps", "application/blok-map-project", "Открыть в MapViewer", "\"%1\"")
               {
                   Icon        = "gpsfile.ico",
                   Description = "Карта КЛУБ/БЛОК"
               });
    }
}