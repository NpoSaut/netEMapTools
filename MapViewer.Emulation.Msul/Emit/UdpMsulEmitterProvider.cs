using System;
using System.Reactive.Linq;
using MapViewer.Emulation.Msul.Settings;
using ReactiveUI;

namespace MapViewer.Emulation.Msul.Emit
{
    public class UdpMsulEmitterProvider : IMsulEmitterProvider
    {
        private readonly IMsulEmulationSettings _msulEmulationSettings;

        public UdpMsulEmitterProvider(IMsulEmulationSettings MsulEmulationSettings) { _msulEmulationSettings = MsulEmulationSettings; }

        public IObservable<IMsulEmitter> LeftEmitter
        {
            get
            {
                return _msulEmulationSettings.WhenAnyValue(x => x.LeftEmissionLink)
                                             .Select(link => new UdpMsulEmitter(link));
            }
        }

        public IObservable<IMsulEmitter> RightEmitter
        {
            get
            {
                return _msulEmulationSettings.WhenAnyValue(x => x.RightEmissionLink)
                                             .Select(link => new UdpMsulEmitter(link));
            }
        }
    }
}
