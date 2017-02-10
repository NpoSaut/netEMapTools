﻿using EMapNavigator.Settings.Interfaces;
using Geographics;
using MapViewer.Settings.Interfaces;

namespace EMapNavigator.Settings.Implementations
{
    public class UserSettings : IMapPositionSettings, IMapBehaviorSettings, IMapAppearanceSettings
    {
        public UserSettings()
        {
            MapCenter = new EarthPoint(new Degree(55.729959), new Degree(37.540420));
            ZoomLevel = 14;
            JumpOnOpen = true;
            HighResolutionTiles = false;
        }

        public bool HighResolutionTiles { get; set; }
        public bool JumpOnOpen { get; set; }
        public EarthPoint MapCenter { get; set; }
        public int ZoomLevel { get; set; }
    }
}
