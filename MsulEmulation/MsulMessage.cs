namespace MsulEmulation
{
    public class MsulMessage
    {
        public MsulMessage(byte[] Data) { this.Data = Data; }
        public byte[] Data { get; private set; }
    }
}