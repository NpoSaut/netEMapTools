using System.Net;
using MapViewer.Emulation.Msul.Emit;
using MapViewer.Emulation.Msul.Settings;
using Newtonsoft.Json;
using ReactiveUI;

namespace EMapNavigator.Settings.Implementations
{
    [JsonObject(MemberSerialization.OptIn)]
    public class MsulEmulationSettings : ReactiveObject, IMsulEmulationSettings
    {
        private EmissionLink _leftEmissionLink;
        private EmissionLink _rightEmissionLink;
        private int _testValue;

        public MsulEmulationSettings()
        {
            _testValue = 777;
            _leftEmissionLink = new EmissionLink(new IPAddress(new byte[] { 192, 168, 0, 1 }),
                                                 new IPAddress(new byte[] { 192, 168, 0, 2 }));
            _rightEmissionLink = new EmissionLink(new IPAddress(new byte[] { 192, 168, 0, 1 }),
                                                  new IPAddress(new byte[] { 192, 168, 0, 3 }));
        }

        [JsonProperty]
        public int TestValue
        {
            get { return _testValue; }
            set { this.RaiseAndSetIfChanged(ref _testValue, value); }
        }

        [JsonProperty]
        public EmissionLink LeftEmissionLink
        {
            get { return _leftEmissionLink; }
            set { this.RaiseAndSetIfChanged(ref _leftEmissionLink, value); }
        }

        [JsonProperty]
        public EmissionLink RightEmissionLink
        {
            get { return _rightEmissionLink; }
            set { this.RaiseAndSetIfChanged(ref _rightEmissionLink, value); }
        }
    }
}
