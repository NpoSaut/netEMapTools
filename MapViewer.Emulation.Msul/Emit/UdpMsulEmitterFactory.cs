using MapViewer.Emulation.Msul.Encoding;

namespace MapViewer.Emulation.Msul.Emit
{
    public class UdpMsulEmitterFactory : IMsulEmitterFactory
    {
        private readonly IMsulMessageEncoder _encoder;

        public UdpMsulEmitterFactory(IMsulMessageEncoder Encoder)
        {
            _encoder = Encoder;
        }

        public IMsulEmitter Create(EmissionLink ForLink)
        {
            return new UdpMsulEmitter(ForLink, _encoder);
        }
    }
}