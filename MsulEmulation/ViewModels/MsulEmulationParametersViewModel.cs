using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using Geographics;
using MapViewer.Emulation;
using MapViewer.Emulation.Wheels;
using MsulEmulation.Entities;
using ReactiveUI;
using Tracking;

namespace MsulEmulation.ViewModels
{
    public class MsulEmulationParametersViewModel : ReactiveObject
    {
        private readonly ObservableAsPropertyHelper<EarthPoint> _position;
        private readonly ObservableAsPropertyHelper<double> _speed;
        private readonly ObservableAsPropertyHelper<DateTime> _time;
        private double _altitude;
        private bool _emergencyStop;
        private bool _leftDoorLocked;
        private bool _leftDoorOpened;
        private bool _lightOn;
        private double _outdoorTemperature;
        private bool _rightDoorLocked;
        private bool _rightDoorOpened;
        private TimeSpan _timeShift;
        private int _trainNumber;

        public MsulEmulationParametersViewModel(IWheel Wheel, IPathRiderProvider PathRiderProvider)
        {
            TimeShift = TimeSpan.FromHours(15) - DateTime.Now.TimeOfDay;

            TrainNumber = 777;
            OutdoorTemperature = -18.4;
            EmergencyStop = false;
            LeftDoorLocked = true;
            RightDoorLocked = false;
            LeftDoorOpened = false;
            RightDoorOpened = false;
            LightOn = true;

            Carriages =
                new[]
                {
                    new CarriageParametersViewModel(1, CarriageKind.TractionHead),
                    new CarriageParametersViewModel(2, CarriageKind.HighVoltage),
                    new CarriageParametersViewModel(3, CarriageKind.Normal),
                    new CarriageParametersViewModel(4, CarriageKind.HighVoltage),
                    new CarriageParametersViewModel(5, CarriageKind.TractionHead)
                };

            Observable.Interval(TimeSpan.FromSeconds(1))
                      .Select(i => DateTime.Now + TimeShift)
                      .ToProperty(this, x => x.Time, out _time);

            Observable.FromEvent(a => Wheel.SpeedChanged += (s, e) => a(), a => { })
                      .Select(_ => Wheel.Speed)
                      .ToProperty(this, x => x.Speed, out _speed);

            PathRiderProvider.PathRider
                             .CombineLatest(Wheel.Milage,
                                            (rider, milage) => rider.PointAt(milage))
                             .ToProperty(this, x => x.Position, out _position);
        }

        public DateTime Time
        {
            get { return _time.Value; }
        }

        public TimeSpan TimeShift
        {
            get { return _timeShift; }
            set { this.RaiseAndSetIfChanged(ref _timeShift, value); }
        }

        public double OutdoorTemperature
        {
            get { return _outdoorTemperature; }
            set { this.RaiseAndSetIfChanged(ref _outdoorTemperature, value); }
        }

        public bool EmergencyStop
        {
            get { return _emergencyStop; }
            set { this.RaiseAndSetIfChanged(ref _emergencyStop, value); }
        }

        public bool LeftDoorLocked
        {
            get { return _leftDoorLocked; }
            set { this.RaiseAndSetIfChanged(ref _leftDoorLocked, value); }
        }

        public bool RightDoorLocked
        {
            get { return _rightDoorLocked; }
            set { this.RaiseAndSetIfChanged(ref _rightDoorLocked, value); }
        }

        public bool LeftDoorOpened
        {
            get { return _leftDoorOpened; }
            set { this.RaiseAndSetIfChanged(ref _leftDoorOpened, value); }
        }

        public bool RightDoorOpened
        {
            get { return _rightDoorOpened; }
            set { this.RaiseAndSetIfChanged(ref _rightDoorOpened, value); }
        }

        public bool LightOn
        {
            get { return _lightOn; }
            set { this.RaiseAndSetIfChanged(ref _lightOn, value); }
        }

        public int TrainNumber
        {
            get { return _trainNumber; }
            set { this.RaiseAndSetIfChanged(ref _trainNumber, value); }
        }

        public double Speed
        {
            get { return _speed.Value; }
        }

        public EarthPoint Position
        {
            get { return _position.Value; }
        }

        public double Altitude
        {
            get { return _altitude; }
            set { this.RaiseAndSetIfChanged(ref _altitude, value); }
        }

        public IList<CarriageParametersViewModel> Carriages { get; private set; }
    }
}
