using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
        private ReactiveObject _activePage;

        public MainViewModel()
        {
            var constructor = new ConstructorViewModel();
            _activePage = constructor;

            this.WhenAnyValue(x => x.ActivePage)
                .OfType<ConstructorViewModel>()
                .Select(c => c.Commit.Select(_ => c.TrainViewModel))
                .Switch()
                .Subscribe(SwitchToTrainModel);
        }

        public ReactiveObject ActivePage
        {
            get => _activePage;
            set => this.RaiseAndSetIfChanged(ref _activePage, value);
        }

        private void SwitchToTrainModel(List<List<CarriageParametersViewModel>> Train)
        {
            var parameters = new MsulEmulationParametersViewModel(new FakeWheel(),
                                                                  new FakePathRider(),
                                                                  Train.SelectMany(x => x).ToList());

            var emulation = new EmulatorViewModel(parameters);

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