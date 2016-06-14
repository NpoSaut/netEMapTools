using System;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Geographics;
using MapViewer.Mapping;
using Microsoft.Win32;
using ReactiveUI;
using Tracking.Formatters;
using Tracking.Presenting;

namespace Tracking.ViewModels
{
    public class TrackingControlViewModel : ReactiveObject
    {
        private const string _fileTypes = "GPX File (*.gpx)|*.gpx|AnyFile (*.*)|*.*";
        private readonly ITrackFormatter _trackFormatter;
        private readonly ITrackPresenter _trackPresenter;
        private IMappingService _mappingService;
        private IDisposable _previousTrackDisplaying;
        private GpsTrack _track;

        public TrackingControlViewModel(ITrackFormatter TrackFormatter, ITrackPresenter TrackPresenter, IMappingService MappingService)
        {
            _trackFormatter = TrackFormatter;
            _trackPresenter = TrackPresenter;
            _mappingService = MappingService;

            LoadTrack = ReactiveCommand.CreateAsyncTask(LoadTrackImpl);
            SaveTrack = ReactiveCommand.CreateAsyncTask(SaveTrackImpl);

            _track = new GpsTrack();

            _mappingService.Clicks
                           .Where(c => c.Action == MouseAction.LeftClick)
                           .Subscribe(c => AppendPointToTrack(c.Point));

            _mappingService.Clicks
                           .Where(c => c.Action == MouseAction.RightClick)
                           .Subscribe(c =>
                                      {
                                          _track = new GpsTrack();
                                          RefreshTrack();
                                      });
        }

        public ICommand LoadTrack { get; private set; }
        public ICommand SaveTrack { get; private set; }

        private void AppendPointToTrack(EarthPoint Point)
        {
            _track.TrackPoints.Add(Point);
            RefreshTrack();
        }

        private void RefreshTrack()
        {
            if (_previousTrackDisplaying != null)
                _previousTrackDisplaying.Dispose();

            if (_track.TrackPoints.Any())
                _previousTrackDisplaying = _trackPresenter.DisplayTrack(_track);
        }

        private async Task SaveTrackImpl(object _)
        {
            var dlg = new SaveFileDialog { DefaultExt = "gpx", Filter = _fileTypes, FilterIndex = 0 };
            if (dlg.ShowDialog() != true)
                return;

            await Task.Run(() =>
                           {
                               using (FileStream stream = File.Create(dlg.FileName))
                               {
                                   _trackFormatter.SaveTrack(_track, stream);
                               }
                           });
        }

        private async Task LoadTrackImpl(object _)
        {
            var dlg = new OpenFileDialog { DefaultExt = "gpx", Filter = _fileTypes, FilterIndex = 0 };
            if (dlg.ShowDialog() != true)
                return;

            _track = await Task.Run(() =>
                                    {
                                        using (FileStream stream = File.OpenRead(dlg.FileName))
                                        {
                                            return _trackFormatter.LoadTrack(stream);
                                        }
                                    });
            RefreshTrack();
        }
    }
}
