﻿using EMapNavigator.Settings.Interfaces;
using Geographics;
using MapViewer.Settings.Interfaces;
using Newtonsoft.Json;
using ReactiveUI;

namespace EMapNavigator.Settings.Implementations
{
    [JsonObject(MemberSerialization.OptIn)]
    public class UserSettings : ReactiveObject, IMapPositionSettings, IMapBehaviorSettings, IMapAppearanceSettings
    {
        private bool _highResolutionTiles;

        public UserSettings()
        {
            MapCenter = new EarthPoint(new Degree(55.729959), new Degree(37.540420));
            ZoomLevel = 14;
            JumpOnOpen = true;
            HighResolutionTiles = false;
        }

        [JsonProperty]
        public bool HighResolutionTiles
        {
            get { return _highResolutionTiles; }
            set { this.RaiseAndSetIfChanged(ref _highResolutionTiles, value); }
        }

        [JsonProperty]
        public bool JumpOnOpen { get; set; }

        [JsonProperty]
        public EarthPoint MapCenter { get; set; }

        [JsonProperty]
        public int ZoomLevel { get; set; }
    }
}
