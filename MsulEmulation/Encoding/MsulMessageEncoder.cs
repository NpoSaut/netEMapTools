using System;
using System.Collections.Generic;
using System.IO;
using MsulEmulation.Entities;
using MsulEmulation.ViewModels;

namespace MsulEmulation.Encoding
{
    public class MsulMessageEncoder : IMsulMessageEncoder
    {
        private const double LatitudeFactor = 0.00000083819;

        private readonly Dictionary<CarriageKind, byte> _carriageKinds =
            new Dictionary<CarriageKind, byte>
            {
                { CarriageKind.TractionHead, 1 },
                { CarriageKind.HighVoltage, 2 },
                { CarriageKind.Normal, 3 },
            };

        private readonly DateTime _zeroTime = new DateTime(1970, 1, 1);

        public MsulMessage GetMessage(MsulEmulationParametersViewModel ViewModel)
        {
            using (var ms = new MemoryStream())
            {
                var writer = new BinaryWriter(ms);

                writer.Write((Byte)1);
                writer.Write((Byte)1);
                writer.Write((Byte)ViewModel.Carriages.Count);
                writer.Write((UInt32)(ViewModel.Time - _zeroTime).TotalSeconds);
                writer.Write((UInt16)ViewModel.TrainNumber);
                writer.Write((Byte)ViewModel.Speed);
                writer.Write(EncodeTemperature(ViewModel.OutdoorTemperature));
                writer.Write((Int32)(ViewModel.Position.Latitude.Value * LatitudeFactor));
                writer.Write((Int32)(ViewModel.Position.Longitude.Value * LatitudeFactor));
                writer.Write((Int16)ViewModel.Altitude);
                writer.Write((Byte)10);
                writer.Write((Byte)((ViewModel.EmergencyStop ? 1 : 0) << 0 |
                                    (ViewModel.LeftDoorLocked ? 0 : 1) << 1 |
                                    (ViewModel.RightDoorLocked ? 0 : 1) << 2 |
                                    (ViewModel.LeftDoorOpened ? 1 : 0) << 3 |
                                    (ViewModel.RightDoorOpened ? 1 : 0) << 4 |
                                    (ViewModel.LightOn ? 1 : 0) << 5));
                writer.Write((Byte)1);

                writer.Write((Byte)carriage.Number);
                writer.Write(_carriageKinds[carriage.Kind]);
                writer.Write(EncodeTemperature(carriage.IndoorTemperature));
                writer.Write((Byte)((carriage.EmergencyValueReleased ? 1 : 0) << 0 |
                                    (carriage.Toilet1Occupied ? 0 : 1) << 1 |
                                    (carriage.Toilet2Occupied ? 0 : 1) << 2));
                writer.Write((Byte)0);
                writer.Write((Byte)0);

                byte[] xxx = ms.ToArray();

                return new MsulMessage(ms.ToArray());
            }
        }

        private byte EncodeTemperature(double TemperatureValue) { return (byte)(TemperatureValue - 60); }
    }
}
