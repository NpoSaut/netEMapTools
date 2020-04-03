using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Timers;
using System.Windows;
using EMapNavigator.Emition;
using EMapNavigator.ViewModels;
using Geographics;
using MapVisualization.Elements;
using Tracking;
using Tracking.MapElements;

namespace EMapNavigator
{
    /// <summary>Логика взаимодействия для MainWindow.xaml</summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}