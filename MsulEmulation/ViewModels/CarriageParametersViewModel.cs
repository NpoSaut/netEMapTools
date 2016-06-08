using MsulEmulation.Entities;
using ReactiveUI;

namespace MsulEmulation.ViewModels
{
    public class CarriageParametersViewModel : ReactiveObject
    {
        public CarriageParametersViewModel(int Number, CarriageKind Kind)
        {
            this.Number = Number;
            this.Kind = Kind;
            IndoorTemperature = 24.0;
            EmergencyValueReleased = false;
            Toilet1Occupied = false;
            Toilet2Occupied = false;
        }

        public int Number { get; set; }
        public CarriageKind Kind { get; private set; }
        public double IndoorTemperature { get; set; }
        public bool EmergencyValueReleased { get; set; }
        public bool Toilet1Occupied { get; set; }
        public bool Toilet2Occupied { get; set; }
    }
}
