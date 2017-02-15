using System;
using System.Net;
using MapViewer.Emulation.Msul.Emit;
using MapViewer.Emulation.Msul.Settings;
using ReactiveUI;

namespace MapViewer.Emulation.Msul.ViewModels
{
    public class MsulEmulationSettingsViewModel : ReactiveObject
    {
        private readonly IMsulEmulationSettings _emulationSettings;

        public MsulEmulationSettingsViewModel(IMsulEmulationSettings EmulationSettings)
        {
            _emulationSettings = EmulationSettings;

            Left = new MsulEmissionLinkViewModel(_emulationSettings.LeftEmissionLink.LocalAddress,
                                                 _emulationSettings.LeftEmissionLink.TargetAddress);
            Right = new MsulEmissionLinkViewModel(_emulationSettings.RightEmissionLink.LocalAddress,
                                                  _emulationSettings.RightEmissionLink.TargetAddress);

            this.WhenAnyValue(x => x.Left.From, x => x.Left.To,
                              (from, to) => new EmissionLink(from, to))
                .Subscribe(link => _emulationSettings.LeftEmissionLink = link);
            this.WhenAnyValue(x => x.Right.From, x => x.Right.To,
                              (from, to) => new EmissionLink(from, to))
                .Subscribe(link => _emulationSettings.RightEmissionLink = link);
        }

        public MsulEmissionLinkViewModel Right { get; private set; }

        public MsulEmissionLinkViewModel Left { get; private set; }
    }

    public class MsulEmissionLinkViewModel : ReactiveObject
    {
        private IPAddress _from;
        private IPAddress _to;

        public MsulEmissionLinkViewModel(IPAddress From, IPAddress To)
        {
            _from = From;
            _to = To;
        }

        public IPAddress From
        {
            get { return _from; }
            set { this.RaiseAndSetIfChanged(ref _from, value); }
        }

        public IPAddress To
        {
            get { return _to; }
            set { this.RaiseAndSetIfChanged(ref _to, value); }
        }
    }
}
