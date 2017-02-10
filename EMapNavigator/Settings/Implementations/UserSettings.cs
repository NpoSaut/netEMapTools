using EMapNavigator.Settings.Interfaces;
using Geographics;

namespace EMapNavigator.Settings.Implementations
{
    public class UserSettings : IMapPositionSettings
    {
        public UserSettings()
        {
            MapCenter = new EarthPoint(new Degree(55.729959), new Degree(37.540420));
            ZoomLevel = 14;
        }

        public EarthPoint MapCenter { get; set; }
        public int ZoomLevel { get; set; }
    }
}
