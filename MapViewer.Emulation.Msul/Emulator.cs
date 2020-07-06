using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using MapViewer.Emulation.Msul.Emit;
using MapViewer.Emulation.Msul.Encoding;

namespace MapViewer.Emulation.Msul
{
    public class Emulator
    {
        private readonly IMsulEmitterFactory _emitterFactory;

        public Emulator(IMsulEmitterFactory EmitterFactory)
        {
            _emitterFactory = EmitterFactory;
        }

        public IDisposable Run(IList<EmulationTarget> Targets)
        {
            var emitters = Targets.Select(t =>
                                   {
                                       var emitter = _emitterFactory.Create(t.Link);
                                       return t.Data.EmitOn(emitter);
                                   })
                                  .ToList();

            return new CompositeDisposable(emitters);
        }
    }

    public class EmulationTarget
    {
        public EmulationTarget(EmissionLink Link, IObservable<MsulData> Data)
        {
            this.Link = Link;
            this.Data = Data;
        }

        public EmissionLink          Link { get; }
        public IObservable<MsulData> Data { get; }
    }
}