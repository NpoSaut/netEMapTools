using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MapViewer;

namespace EMapNavigator
{
    public class MainMenuService : IMainMenuService
    {
        private readonly Menu _menu;

        public MainMenuService()
        {
            _menu = ((MainWindow)Application.Current.MainWindow).MainMenu;
        }

        public void RegisterCommand(MenuPath Path, ICommand Command)
        {
            var item = FindMenuItem(Path.Segments);
            item.Command = Command;
        }

        public void RegisterCheckbox(MenuPath Path, Func<bool> Getter, Action<bool> Setter)
        {
            var item = FindMenuItem(Path.Segments);
            item.IsCheckable =  true;
            item.IsChecked   =  Getter();
            item.Click       += (s, e) => Setter(item.IsChecked);
        }

        private MenuItem FindMenuItem(IList<string> Path)
        {
            MenuItem cursor = null;
            var      items  = _menu.Items;
            foreach (var elementName in Path)
            {
                cursor = items.OfType<MenuItem>().SingleOrDefault(i => (string)i.Header == elementName);
                if (cursor == null)
                {
                    cursor = new MenuItem { Header = elementName };
                    items.Add(cursor);
                }

                items = cursor.Items;
            }

            return cursor;
        }
    }
}
