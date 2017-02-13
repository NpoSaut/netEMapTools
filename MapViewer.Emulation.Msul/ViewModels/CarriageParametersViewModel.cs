using MapViewer.Emulation.Msul.Entities;
using ReactiveUI;

namespace MapViewer.Emulation.Msul.ViewModels
{
    public class CarriageParametersViewModel : ReactiveObject
    {
        private bool _emergencyValueReleased;
        private double _indoorTemperature;
        private bool _toilet1Occupied;
        private bool _toilet2Occupied;

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

        public double IndoorTemperature
        {
            get { return _indoorTemperature; }
            set { this.RaiseAndSetIfChanged(ref _indoorTemperature, value); }
        }

        public bool EmergencyValueReleased
        {
            get { return _emergencyValueReleased; }
            set { this.RaiseAndSetIfChanged(ref _emergencyValueReleased, value); }
        }

        public bool Toilet1Occupied
        {
            get { return _toilet1Occupied; }
            set { this.RaiseAndSetIfChanged(ref _toilet1Occupied, value); }
        }

        public bool Toilet2Occupied
        {
            get { return _toilet2Occupied; }
            set { this.RaiseAndSetIfChanged(ref _toilet2Occupied, value); }
        }
    }
}
