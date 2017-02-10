using Geographics;

namespace EMapNavigator.Settings.Interfaces
{
    public interface IMapPositionSettings
    {
        EarthPoint MapCenter { get; set; }
        int ZoomLevel { get; set; }
    }
}