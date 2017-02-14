using Geographics;
using MapViewer.Settings.Interfaces;

namespace EMapNavigator.Settings.Interfaces
{
    public interface IMapPositionSettings : ISettings
    {
        EarthPoint MapCenter { get; set; }
        int ZoomLevel { get; set; }
    }
}
