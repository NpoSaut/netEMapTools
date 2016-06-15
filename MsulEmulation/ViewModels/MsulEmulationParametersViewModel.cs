using System;
using System.Collections.Generic;
using MsulEmulation.Entities;
using ReactiveUI;

namespace MsulEmulation.ViewModels
{
    public class MsulEmulationParametersViewModel : ReactiveObject
    {
        public MsulEmulationParametersViewModel()
        {
            TimeShift = TimeSpan.FromHours(15) - DateTime.Now.TimeOfDay;
            Time = DateTime.Now + TimeShift;

            OutdoorTemperature = -18.4;
            EmergencyStop = false;
            LeftDoorLocked = false;
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
        }

        public DateTime Time { get; set; }
        public TimeSpan TimeShift { get; private set; }
        public double OutdoorTemperature { get; set; }
        public bool EmergencyStop { get; set; }
        public bool LeftDoorLocked { get; set; }
        public bool RightDoorLocked { get; set; }
        public bool LeftDoorOpened { get; set; }
        public bool RightDoorOpened { get; set; }
        public bool LightOn { get; set; }
        public IList<CarriageParametersViewModel> Carriages { get; private set; }
        private DateTime CorrectTime() { return Time = DateTime.Now + TimeShift; }
    }
}
