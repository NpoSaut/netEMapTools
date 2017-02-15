﻿using System;
using System.Windows;
using System.Windows.Input;
using EMapNavigator.Views;
using ReactiveUI;

namespace EMapNavigator.ViewModels
{
    public class SettingsToolbatViewModel : ReactiveObject
    {
        public SettingsToolbatViewModel()
        {
            ReactiveCommand<object> showSettingsDialogCommand = ReactiveCommand.Create();
            ShowSettingsDialog = showSettingsDialogCommand;

            showSettingsDialogCommand.Subscribe(_ => new SettingsWindow { Owner = Application.Current.MainWindow }.Show());
        }

        public ICommand ShowSettingsDialog { get; private set; }
    }
}