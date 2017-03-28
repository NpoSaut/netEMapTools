using System;
using System.Collections.Generic;
using MapViewer.Emulation.Blok.Emission.Options;

namespace MapViewer.Emulation.Blok.Emission
{
    public interface IBlokEmitterFactory
    {
        string Name { get; }
        IBlokEmitter CreatEmitter(ICollection<IEmissionOption> Options, IObservable<NavigationInformation> Navigation);
    }
}
