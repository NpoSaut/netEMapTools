namespace Tracking.Formatters.CanLog
{
    public struct CanFrameLogRecord
    {
        public CanFrameLogRecord(ushort Descriptor, byte[] Data)
        {
            this.Descriptor = Descriptor;
            this.Data       = Data;
        }

        public ushort Descriptor { get; }
        public byte[] Data       { get; }
    }
}