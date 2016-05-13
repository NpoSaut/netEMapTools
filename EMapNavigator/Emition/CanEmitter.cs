using BlokFrames;
using Communications.Appi;
using Communications.Can;
using Geographics;

namespace EMapNavigator.Emition
{
    public class CanEmitter : IEmitter
    {
        private readonly AppiDev _appiDevice;
        public CanEmitter(AppiDev AppiDevice) { _appiDevice = AppiDevice; }

        public void EmitPosition(EarthPoint Position, double Speed)
        {
            CanFrame frame = new MmAltLongFrame(Position.Latitude, Position.Longitude).GetCanFrame();
            CanFrame fx = CanFrame.NewWithId(0x5c0, frame.Data);
            _appiDevice.CanPorts[AppiLine.Can1].Send(fx);
        }
    }
}