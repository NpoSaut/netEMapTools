using System.IO;
using System.Linq;
using System.Windows;
using Geographics;
using GMapElements;

namespace EMapNavigator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GMap gMap;
            using (var mapStream = new FileStream("Екатеринбург.gps", FileMode.Open))
            {
                gMap = GMap.Load(mapStream);
            }

            foreach (var post in gMap.Sections.SelectMany(sec => sec.Posts))
            {
                map.AddElement(new KilometerPostMapElement(post));
            }
        }
    }
}
