namespace EMapNavigator.Settings.Interfaces
{
    public interface ISettingsFactory<out TSettings>
    {
        TSettings Produce();
    }
}
