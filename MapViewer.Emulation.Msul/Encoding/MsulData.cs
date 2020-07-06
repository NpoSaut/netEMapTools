using System;
using System.Collections.Generic;
using Geographics;
using MapViewer.Emulation.Msul.Entities;

namespace MapViewer.Emulation.Msul.Encoding
{
    public class MsulData
    {
        public MsulData(
            InitializationKind InitializationKind, DateTime Time, int TrainNumber, double Speed,
            double OutdoorTemperature, EarthPoint Position, double Altitude, bool EmergencyStop, bool LeftDoorLocked,
            bool RightDoorLocked, bool LeftDoorOpened, bool RightDoorOpened, bool LightOn, IList<Carriage> Carriages)
        {
            this.InitializationKind = InitializationKind;
            this.Time               = Time;
            this.TrainNumber        = TrainNumber;
            this.Speed              = Speed;
            this.OutdoorTemperature = OutdoorTemperature;
            this.Position           = Position;
            this.Altitude           = Altitude;
            this.EmergencyStop      = EmergencyStop;
            this.LeftDoorLocked     = LeftDoorLocked;
            this.RightDoorLocked    = RightDoorLocked;
            this.LeftDoorOpened     = LeftDoorOpened;
            this.RightDoorOpened    = RightDoorOpened;
            this.LightOn            = LightOn;
            this.Carriages          = Carriages;
        }

        public InitializationKind InitializationKind { get; }
        public DateTime           Time               { get; }
        public int                TrainNumber        { get; }
        public double             Speed              { get; }
        public double             OutdoorTemperature { get; }
        public EarthPoint         Position           { get; }
        public double             Altitude           { get; }
        public bool               EmergencyStop      { get; }
        public bool               LeftDoorLocked     { get; }
        public bool               RightDoorLocked    { get; }
        public bool               LeftDoorOpened     { get; }
        public bool               RightDoorOpened    { get; }
        public bool               LightOn            { get; }
        public IList<Carriage>    Carriages          { get; }

        public class Carriage
        {
            public Carriage(
                int Number, CarriageKind Kind, double IndoorTemperature, bool EmergencyValueReleased,
                bool Toilet1Occupied, bool Toilet2Occupied)
            {
                this.Number                 = Number;
                this.Kind                   = Kind;
                this.IndoorTemperature      = IndoorTemperature;
                this.EmergencyValueReleased = EmergencyValueReleased;
                this.Toilet1Occupied        = Toilet1Occupied;
                this.Toilet2Occupied        = Toilet2Occupied;
            }

            public int          Number                 { get; }
            public CarriageKind Kind                   { get; }
            public double       IndoorTemperature      { get; }
            public bool         EmergencyValueReleased { get; }
            public bool         Toilet1Occupied        { get; }
            public bool         Toilet2Occupied        { get; }
        }
    }
}