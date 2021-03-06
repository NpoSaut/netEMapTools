﻿using System;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Windows.Input;
using Geographics;
using MapViewer;
using MapViewer.Mapping;
using MapViewer.Settings.Interfaces;
using Microsoft.Win32;
using ReactiveUI;
using Tracking.Formatters;
using Tracking.Presenting;

namespace Tracking.ViewModels
{
    public class TrackingControlViewModel : ReactiveObject, IPathRiderProvider
    {
        private readonly IMapBehaviorSettings _behaviorSettings;
        private readonly IMappingService _mappingService;
        private readonly ReactiveList<EarthPoint> _track;

        private readonly TrackFormatterManager _trackFormatterManager;
        private readonly ITrackPresenter _trackPresenter;
        private IDisposable _previousTrackDisplaying;

        public TrackingControlViewModel(ITrackPresenter TrackPresenter, IMappingService MappingService, TrackFormatterManager TrackFormatterManager,
                                        IMapBehaviorSettings BehaviorSettings, IMainMenuService MenuService)
        {
            _trackPresenter = TrackPresenter;
            _trackFormatterManager = TrackFormatterManager;
            _behaviorSettings = BehaviorSettings;
            _mappingService = MappingService;

            _track = new ReactiveList<EarthPoint>();
            _track.Changed
                  .Subscribe(_ => RefreshTrack());

            LoadTrack = ReactiveCommand.CreateAsyncTask(LoadTrackImpl);
            SaveTrack = ReactiveCommand.CreateAsyncTask(SaveTrackImpl);
            ReactiveCommand<object> clearCommand = ReactiveCommand.Create(_track.IsEmptyChanged.Select(e => !e));
            clearCommand.Subscribe(_ => _track.Clear());
            ClearTrack = clearCommand;

            IConnectableObservable<TrackPathRider> prp =
                _track.Changed
                      .Select(_ => new TrackPathRider(new GpsTrack(_track.ToList())))
                      .Publish();
            PathRider = prp;
            prp.Connect();

            _mappingService.Clicks
                           .Where(c => c.Action == MouseAction.LeftClick)
                           .Subscribe(c => AppendPointToTrack(c.Point));

            _mappingService.Clicks
                           .Where(c => c.Action == MouseAction.RightClick)
                           .Subscribe(c => _track.Remove(_track.LastOrDefault()));

            MenuService.RegisterCommand((MenuPath)"Трек" / "Загрузить...", LoadTrack);
            MenuService.RegisterCommand((MenuPath)"Трек" / "Сохранить...", SaveTrack);
            MenuService.RegisterCommand((MenuPath)"Трек" / "Очистить", ClearTrack);
        }

        public ICommand ClearTrack { get; private set; }
        public ICommand LoadTrack { get; private set; }
        public ICommand SaveTrack { get; private set; }
        public IObservable<IPathRider> PathRider { get; private set; }

        private void AppendPointToTrack(EarthPoint Point) { _track.Add(Point); }

        private void RefreshTrack()
        {
            if (_previousTrackDisplaying != null)
                _previousTrackDisplaying.Dispose();

            if (_track.Any())
                _previousTrackDisplaying = _trackPresenter.DisplayTrack(_track);
        }

        private async Task SaveTrackImpl(object _)
        {
            var dlg = new SaveFileDialog { DefaultExt = "gpx", Filter = _trackFormatterManager.GetFileFilterString(FormatterDirection.Save), FilterIndex = 0 };
            if (dlg.ShowDialog() != true)
                return;

            await Task.Run(() =>
                           {
                               using (FileStream stream = File.Create(dlg.FileName))
                               {
                                   _trackFormatterManager.GetFormatter(Path.GetExtension(dlg.FileName))
                                                         .SaveTrack(new GpsTrack(_track), stream);
                               }
                           });
        }

        private async Task LoadTrackImpl(object _)
        {
            var dlg = new OpenFileDialog { DefaultExt = "gpx", Filter = _trackFormatterManager.GetFileFilterString(FormatterDirection.Load), FilterIndex = 0 };
            if (dlg.ShowDialog() != true)
                return;

            _track.Clear();
            GpsTrack track = await Task.Run(() =>
                                            {
                                                using (FileStream stream = File.OpenRead(dlg.FileName))
                                                {
                                                    return _trackFormatterManager.GetFormatter(Path.GetExtension(dlg.FileName))
                                                                                 .LoadTrack(stream);
                                                }
                                            });
            _track.AddRange(track.TrackPoints);

            if (_behaviorSettings.JumpOnOpen)
                _mappingService.Navigate(_track.First());
        }
    }
}
