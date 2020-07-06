using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reactive.Disposables;
using MapViewer.Emulation.Msul;
using MapViewer.Emulation.Msul.Emit;
using MapViewer.Emulation.Msul.Encoding;
using MapViewer.Emulation.Msul.ViewModels;
using ReactiveUI;

namespace SimpleMsul.ViewModels
{
    public class EmulatorViewModel : ReactiveObject, IMainWindowPage
    {
        LocalAddressProvider _localAddressProvider = new LocalAddressProvider();
        private readonly IDictionary<int, IPAddress> _addresses;
        private readonly Emulator _emulator = new Emulator(new UdpMsulEmitterFactory(new MsulMessageEncoder()));

        private readonly CompositeDisposable _cleanUp = new CompositeDisposable();

        public EmulatorViewModel(MsulEmulationParametersViewModel Parameters, IDictionary<int, IPAddress> Addresses)
        {
            _addresses = Addresses;
            this.Parameters = Parameters;

            Run().DisposeWith(_cleanUp);
        }

        public MsulEmulationParametersViewModel Parameters { get; }

        public void Dispose()
        {
            _cleanUp.Dispose();
        }

        private IDisposable Run()
        {
            var targets = Parameters.ToMsulDataAllFlows()
                                    .Select(x => new EmulationTarget(CreateEmitLink(x.Key), x.Value))
                                    .ToList();

            return _emulator.Run(targets);
        }

        private EmissionLink CreateEmitLink(int ForCarNumber)
        {
            var remoteAddress = _addresses[ForCarNumber];
            var localAddress = _localAddressProvider.GetLocalIPAddress(remoteAddress);
            return new EmissionLink(localAddress, remoteAddress);
        }
    }
}