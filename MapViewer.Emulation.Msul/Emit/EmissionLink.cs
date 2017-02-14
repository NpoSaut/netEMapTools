using System.Net;

namespace MapViewer.Emulation.Msul.Emit
{
    public class EmissionLink
    {
        public EmissionLink(IPAddress LocalAddress, IPAddress TargetAddress)
        {
            this.LocalAddress = LocalAddress;
            this.TargetAddress = TargetAddress;
        }

        public IPAddress LocalAddress { get; private set; }
        public IPAddress TargetAddress { get; private set; }
    }
}
