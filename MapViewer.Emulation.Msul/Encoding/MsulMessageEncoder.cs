using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MapViewer.Emulation.Msul.Entities;

namespace MapViewer.Emulation.Msul.Encoding
{
    public class MsulMessageEncoder : IMsulMessageEncoder
    {
        private const double LatitudeFactor = 0.00000083819;

        private readonly Dictionary<CarriageKind, byte> _carriageKinds =
            new Dictionary<CarriageKind, byte>
            {
                { CarriageKind.TractionHead, 1 },
                { CarriageKind.HighVoltage, 2 },
                { CarriageKind.Normal, 3 }
            };

        private readonly Dictionary<InitializationKind, byte> _initializationKinds =
            new Dictionary<InitializationKind, byte>
            {
                { InitializationKind.HeadSection, 1 },
                { InitializationKind.TailSection, 5 },
                { InitializationKind.Uninitialized, 0 }
            };

        private readonly DateTime _zeroTime = new DateTime(1970, 1, 1);

        public byte[] GetMessage(MsulData Data, int Counter)
        {
            var i = Counter % Data.Carriages.Count;
            return GetCommonData(Data).Concat(GetCarriageData(Data.Carriages[i])).ToArray();
        }

        private byte[] GetCommonData(MsulData Data)
        {
            using (var ms = new MemoryStream())
            {
                var writer = new BinaryWriter(ms);

                writer.Write((byte) 1);
                writer.Write(_initializationKinds[Data.InitializationKind]);
                writer.Write((byte) Data.Carriages.Count);
                writer.Write((uint) (Data.Time - _zeroTime).TotalSeconds);
                writer.Write((ushort) Data.TrainNumber);
                writer.Write((byte) Data.Speed);
                writer.Write(EncodeTemperature(Data.OutdoorTemperature));
                writer.Write((int) (Data.Position.Longitude.Value / LatitudeFactor));
                writer.Write((int) (Data.Position.Latitude.Value  / LatitudeFactor));
                writer.Write((short) Data.Altitude);
                writer.Write((byte) 10);
                writer.Write((byte) (((Data.EmergencyStop ? 1 : 0)   << 0) |
                                     ((Data.LeftDoorLocked ? 0 : 1)  << 1) |
                                     ((Data.RightDoorLocked ? 0 : 1) << 2) |
                                     ((Data.LeftDoorOpened ? 1 : 0)  << 3) |
                                     ((Data.RightDoorOpened ? 1 : 0) << 4) |
                                     ((Data.LightOn ? 1 : 0)         << 5)));
                writer.Write((byte) 1);
                return ms.ToArray();
            }
        }

        private byte[] GetCarriageData(MsulData.Carriage carriage)
        {
            using (var ms = new MemoryStream())
            {
                var writer = new BinaryWriter(ms);
                writer.Write((byte) carriage.Number);
                writer.Write(_carriageKinds[carriage.Kind]);
                writer.Write(EncodeTemperature(carriage.IndoorTemperature));
                writer.Write((byte) (((carriage.EmergencyValueReleased ? 1 : 0) << 0) |
                                     ((carriage.Toilet1Occupied ? 1 : 0)        << 1) |
                                     ((carriage.Toilet2Occupied ? 1 : 0)        << 2)));
                writer.Write((byte) 0);
                writer.Write((byte) 0);
                return ms.ToArray();
            }
        }

        private byte EncodeTemperature(double TemperatureValue)
        {
            return (byte) Math.Round(TemperatureValue + 60);
        }
    }
}