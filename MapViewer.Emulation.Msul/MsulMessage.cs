using System.Collections.Generic;

namespace MapViewer.Emulation.Msul
{
    public class MsulMessage
    {
        public MsulMessage(byte[] CommonData, IList<byte[]> CarriagesData)
        {
            this.CarriagesData = CarriagesData;
            this.CommonData = CommonData;
        }

        public byte[] CommonData { get; private set; }
        public IList<byte[]> CarriagesData { get; private set; }
    }
}
