namespace MapViewer.Settings.Interfaces
{
    public interface IMapAppearanceSettings : ISettings
    {
        bool HighResolutionTiles { get; set; }
    }
}