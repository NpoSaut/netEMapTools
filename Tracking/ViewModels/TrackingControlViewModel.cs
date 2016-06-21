using System;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
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
    public class TrackingControlViewModel : ReactiveObject, IPathRiderProvider
    {
        private const string FileTypes = "GPX File (*.gpx)|*.gpx|AnyFile (*.*)|*.*";
        private readonly ReactiveList<EarthPoint> _track;

        private readonly ITrackFormatter _trackFormatter;
        private readonly ITrackPresenter _trackPresenter;
        private IDisposable _previousTrackDisplaying;

        public TrackingControlViewModel(ITrackFormatter TrackFormatter, ITrackPresenter TrackPresenter, IMappingService MappingService)
        {
            _trackFormatter = TrackFormatter;
            _trackPresenter = TrackPresenter;
            IMappingService mappingService = MappingService;

            LoadTrack = ReactiveCommand.CreateAsyncTask(LoadTrackImpl);
            SaveTrack = ReactiveCommand.CreateAsyncTask(SaveTrackImpl);

            _track = new ReactiveList<EarthPoint>();
            _track.Changed
                  .Subscribe(_ => RefreshTrack());

            IConnectableObservable<TrackPathRider> prp =
                _track.Changed
                      .Select(_ => new TrackPathRider(new GpsTrack(_track.ToList())))
                      .Publish();
            PathRider = prp;
            prp.Connect();

            mappingService.Clicks
                          .Where(c => c.Action == MouseAction.LeftClick)
                          .Subscribe(c => AppendPointToTrack(c.Point));

            mappingService.Clicks
                          .Where(c => c.Action == MouseAction.RightClick)
                          .Subscribe(c =>
                                     {
                                         _track.Clear();
                                         RefreshTrack();
                                     });
        }

        public ICommand LoadTrack { get; private set; }
        public ICommand SaveTrack { get; private set; }
        public IObservable<IPathRider> PathRider { get; private set; }

        private void AppendPointToTrack(EarthPoint Point)
        {
            _track.Add(Point);
            RefreshTrack();
        }

        private void RefreshTrack()
        {
            if (_previousTrackDisplaying != null)
                _previousTrackDisplaying.Dispose();

            if (_track.Any())
                _previousTrackDisplaying = _trackPresenter.DisplayTrack(_track);
        }

        private async Task SaveTrackImpl(object _)
        {
            var dlg = new SaveFileDialog { DefaultExt = "gpx", Filter = FileTypes, FilterIndex = 0 };
            if (dlg.ShowDialog() != true)
                return;

            await Task.Run(() =>
                           {
                               using (FileStream stream = File.Create(dlg.FileName))
                               {
                                   _trackFormatter.SaveTrack(new GpsTrack(_track), stream);
                               }
                           });
        }

        private async Task LoadTrackImpl(object _)
        {
            var dlg = new OpenFileDialog { DefaultExt = "gpx", Filter = FileTypes, FilterIndex = 0 };
            if (dlg.ShowDialog() != true)
                return;

            _track.Clear();
            GpsTrack track = await Task.Run(() =>
                                            {
                                                using (FileStream stream = File.OpenRead(dlg.FileName))
                                                {
                                                    return _trackFormatter.LoadTrack(stream);
                                                }
                                            });
            _track.AddRange(track.TrackPoints);

            RefreshTrack();
        }
    }
}
