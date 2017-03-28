using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using MapViewer.Emulation.Blok.Emission.Options;

namespace MapViewer.Emulation.Blok.Emission.Implementations
{
    public abstract class BlokEmitterFactoryBase : IBlokEmitterFactory, IDisposable
    {
        private readonly SerialDisposable _lastCreatedEmitter = new SerialDisposable();

        protected BlokEmitterFactoryBase(string Name) { this.Name = Name; }

        public string Name { get; }

        public IBlokEmitter CreatEmitter(ICollection<IEmissionOption> Options, IObservable<NavigationInformation> Navigation)
        {
            var emitter = ProduceEmitter(Options);
            _lastCreatedEmitter.Disposable = emitter;
            emitter.Emit(Navigation);
            return emitter;
        }

        public void Dispose() { _lastCreatedEmitter.Dispose(); }

        protected abstract IBlokEmitter ProduceEmitter(ICollection<IEmissionOption> Options);
    }
}
