using System.Windows;

namespace MapViewer.Emulation
{
    public static class EmulatorView
    {
        public static readonly DependencyProperty EmulatorNameProperty = DependencyProperty.RegisterAttached(
            "EmulatorName", typeof (string), typeof (EmulatorView), new PropertyMetadata(default(string)));

        public static void SetEmulatorName(DependencyObject element, string value) { element.SetValue(EmulatorNameProperty, value); }

        public static string GetEmulatorName(DependencyObject element) { return (string)element.GetValue(EmulatorNameProperty); }
    }
}
