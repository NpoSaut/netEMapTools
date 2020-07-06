using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Reactive.Linq;
using MapViewer.Emulation.Msul.Emit;
using MapViewer.Emulation.Msul.Settings;
using MapViewer.Emulation.Msul.ViewModels;
using ReactiveUI;
using SimpleMsul.Model;

namespace SimpleMsul.ViewModels
{
    public class MainViewModel : ReactiveObject
    {
        private IMainWindowPage _activePage;

        public MainViewModel()
        {
            var constructor = new ConstructorViewModel();
            _activePage = constructor;

            this.WhenAnyValue(x => x.ActivePage)
                .OfType<ConstructorViewModel>()
                .Select(c => c.Commit.Select(_ => Tuple.Create(c.TrainViewModel, c.AddressesDictionary)))
                .Switch()
                .Subscribe(SwitchToTrainModel);
        }

        public IMainWindowPage ActivePage
        {
            get => _activePage;
            set
            {
                _activePage?.Dispose();
                this.RaiseAndSetIfChanged(ref _activePage, value);
            }
        }

        private void SwitchToTrainModel(Tuple<IList<CarriageParametersViewModel>, IDictionary<int, IPAddress>> Train)
        {
            var parameters = new MsulEmulationParametersViewModel(new FakeWheel(),
                                                                  new FakePathRider(),
                                                                  Train.Item1);

            var emulation = new EmulatorViewModel(parameters, Train.Item2);

            ActivePage = emulation;
        }
    }

    internal class LinkProvider : IMsulEmulationSettings
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public EmissionLink                      LeftEmissionLink  { get; set; }
        public EmissionLink                      RightEmissionLink { get; set; }
    }
}