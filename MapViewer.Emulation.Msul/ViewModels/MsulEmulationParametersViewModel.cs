using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using Geographics;
using MapViewer.Emulation.Msul.Entities;
using MapViewer.Emulation.Wheels;
using ReactiveUI;
using Tracking;

namespace MapViewer.Emulation.Msul.ViewModels
{
    public class MsulEmulationParametersViewModel : ReactiveObject
    {
        private readonly ObservableAsPropertyHelper<bool> _isLeftSectionActive;
        private readonly ObservableAsPropertyHelper<bool> _isRightSectionActive;
        private readonly ObservableAsPropertyHelper<EarthPoint> _position;
        private readonly ObservableAsPropertyHelper<double> _speed;
        private readonly ObservableAsPropertyHelper<DateTime> _time;
        private readonly IWheel _wheel;
        private int _activeSection;
        private double _altitude;

        private readonly IList<CarriageParametersViewModel> _defaultTrainConfiguration =
            new[]
            {
                new CarriageParametersViewModel(1, CarriageKind.TractionHead, CarriagePosition.Left),
                new CarriageParametersViewModel(2, CarriageKind.HighVoltage,  CarriagePosition.Middle),
                new CarriageParametersViewModel(3, CarriageKind.Normal,       CarriagePosition.Middle),
                new CarriageParametersViewModel(4, CarriageKind.HighVoltage,  CarriagePosition.Middle),
                new CarriageParametersViewModel(5, CarriageKind.TractionHead, CarriagePosition.Right)
            };

        private bool _emergencyStop;
        private bool _leftDoorLocked;
        private bool _leftDoorOpened;
        private bool _lightOn;
        private double _outdoorTemperature;
        private bool _rightDoorLocked;
        private bool _rightDoorOpened;
        private TimeSpan _timeShift;
        private int _trainNumber;

        public MsulEmulationParametersViewModel(
            IWheel Wheel, IPathRiderProvider PathRiderProvider, IList<CarriageParametersViewModel> Carriages = null)
        {
            _wheel    = Wheel;
            TimeShift = TimeSpan.FromHours(15) - DateTime.Now.TimeOfDay;

            TrainNumber        = 777;
            OutdoorTemperature = -18.4;
            EmergencyStop      = false;
            LeftDoorLocked     = true;
            RightDoorLocked    = false;
            LeftDoorOpened     = false;
            RightDoorOpened    = false;
            LightOn            = true;

            this.Carriages = Carriages ?? _defaultTrainConfiguration;

            Observable.Interval(TimeSpan.FromSeconds(1))
                      .Select(i => DateTime.Now + TimeShift)
                      .ToProperty(this, x => x.Time, out _time);

            Observable.FromEvent(a => _wheel.SpeedChanged += (s, e) => a(), a => { })
                      .Select(_ => _wheel.Speed)
                      .ToProperty(this, x => x.Speed, out _speed);

            PathRiderProvider.PathRider
                             .CombineLatest(_wheel.Milage,
                                            (rider, milage) => rider.PointAt(milage))
                             .ToProperty(this, x => x.Position, out _position);

            ActiveSection = 1;
            this.WhenAnyValue(x => x.ActiveSection)
                .Select(s => s == 1)
                .ToProperty(this, x => x.IsLeftSectionActive, out _isLeftSectionActive);
            this.WhenAnyValue(x => x.ActiveSection)
                .Select(s => s == 5)
                .ToProperty(this, x => x.IsRightSectionActive, out _isRightSectionActive);
        }

        public bool IsLeftSectionActive
        {
            get => _isLeftSectionActive.Value;
            set => ActiveSection = 1;
        }

        public bool IsRightSectionActive
        {
            get => _isRightSectionActive.Value;
            set => ActiveSection = 5;
        }

        public DateTime Time => _time.Value;

        public TimeSpan TimeShift
        {
            get => _timeShift;
            set => this.RaiseAndSetIfChanged(ref _timeShift, value);
        }

        public double OutdoorTemperature
        {
            get => _outdoorTemperature;
            set => this.RaiseAndSetIfChanged(ref _outdoorTemperature, value);
        }

        public bool EmergencyStop
        {
            get => _emergencyStop;
            set => this.RaiseAndSetIfChanged(ref _emergencyStop, value);
        }

        public bool LeftDoorLocked
        {
            get => _leftDoorLocked;
            set => this.RaiseAndSetIfChanged(ref _leftDoorLocked, value);
        }

        public bool RightDoorLocked
        {
            get => _rightDoorLocked;
            set => this.RaiseAndSetIfChanged(ref _rightDoorLocked, value);
        }

        public bool LeftDoorOpened
        {
            get => _leftDoorOpened;
            set => this.RaiseAndSetIfChanged(ref _leftDoorOpened, value);
        }

        public bool RightDoorOpened
        {
            get => _rightDoorOpened;
            set => this.RaiseAndSetIfChanged(ref _rightDoorOpened, value);
        }

        public bool LightOn
        {
            get => _lightOn;
            set => this.RaiseAndSetIfChanged(ref _lightOn, value);
        }

        public int TrainNumber
        {
            get => _trainNumber;
            set => this.RaiseAndSetIfChanged(ref _trainNumber, value);
        }

        public double Speed => _speed.Value;

        public EarthPoint Position => _position.Value;

        public double Altitude
        {
            get => _altitude;
            set => this.RaiseAndSetIfChanged(ref _altitude, value);
        }

        public IList<CarriageParametersViewModel> Carriages { get; }

        public int ActiveSection
        {
            get => _activeSection;
            set => this.RaiseAndSetIfChanged(ref _activeSection, value);
        }
    }
}