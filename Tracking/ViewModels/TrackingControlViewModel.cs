using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32;
using ReactiveUI;
using Tracking.Formatters;
using Tracking.Presenting;

namespace Tracking.ViewModels
{
    public class TrackingControlViewModel : ReactiveObject
    {
        private readonly ITrackFormatter _trackFormatter;
        private readonly ITrackPresenter _trackPresenter;
        private IDisposable _previousTrackDisplaying;
        private GpsTrack _track;

        public TrackingControlViewModel(ITrackFormatter TrackFormatter, ITrackPresenter TrackPresenter)
        {
            _trackFormatter = TrackFormatter;
            _trackPresenter = TrackPresenter;

            _track = new GpsTrack();

            LoadTrack = ReactiveCommand.CreateAsyncTask(LoadTrackImpl);
            SaveTrack = ReactiveCommand.CreateAsyncTask(SaveTrackImpl);
        }

        public ICommand LoadTrack { get; private set; }
        public ICommand SaveTrack { get; private set; }

        private async Task SaveTrackImpl(object _)
        {
            var dlg = new SaveFileDialog { DefaultExt = "gpx" };
            if (dlg.ShowDialog() != true)
                return;
        }

        private async Task LoadTrackImpl(object _)
        {
            var dlg = new OpenFileDialog { DefaultExt = "gpx" };
            if (dlg.ShowDialog() != true)
                return;

            _track = await Task.Run(() =>
                                    {
                                        using (FileStream stream = File.OpenRead(dlg.FileName))
                                        {
                                            return _trackFormatter.LoadTrack(stream);
                                        }
                                    });

            if (_previousTrackDisplaying != null)
                _previousTrackDisplaying.Dispose();

            _previousTrackDisplaying = _trackPresenter.DisplayTrack(_track);
        }
    }
}
