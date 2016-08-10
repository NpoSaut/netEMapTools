using System;
using System.Reactive.Linq;
using BlokFrames;
using Communications.Appi;
using Communications.Can;
using Geographics;
using MapViewer.Emulation.Blok.Can;

namespace MapViewer.Emulation.Blok
{
    public interface IBlokEmitter
    {
        IObservable<NavigationInformation> Emit(IObservable<NavigationInformation> Navigation);
    }

    public static class BlokEmitterHelper
    {
        public static IObservable<NavigationInformation> EmitThorough(this IObservable<NavigationInformation> Navigation, IBlokEmitter Emitter)
        {
            return Emitter.Emit(Navigation);
        }
    }

    public class NavigationInformation
    {
        public NavigationInformation(EarthPoint Position, double Speed)
        {
            this.Position = Position;
            this.Speed = Speed;
        }

        public EarthPoint Position { get; private set; }
        public double Speed { get; private set; }
    }

    public class CanBlokEmitter : IBlokEmitter
    {
        private readonly IAppiDeviceFactory _appiDeviceFactory;
        public CanBlokEmitter(IAppiDeviceFactory AppiDeviceFactory) { _appiDeviceFactory = AppiDeviceFactory; }

        public IObservable<NavigationInformation> Emit(IObservable<NavigationInformation> Navigation)
        {
            AppiDev appi = _appiDeviceFactory.GetDevice();
            return Navigation.Do(n => Emit(appi, n));
        }

        private void Emit(AppiDev Appi, NavigationInformation NavigationInformation)
        {
            CanFrame frame = new MmAltLongFrame(NavigationInformation.Position.Latitude,
                                                NavigationInformation.Position.Longitude).GetCanFrame();
            CanFrame fx = CanFrame.NewWithId(0x5c0, frame.Data);
            Appi.CanPorts[AppiLine.Can1].Send(fx);
        }
    }
}
