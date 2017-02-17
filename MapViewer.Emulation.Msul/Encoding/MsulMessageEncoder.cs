using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MapViewer.Emulation.Msul.Entities;
using MapViewer.Emulation.Msul.ViewModels;

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
                { CarriageKind.Normal, 3 },
            };

        private readonly Dictionary<InitializationKind, byte> _initializationKinds =
            new Dictionary<InitializationKind, byte>
            {
                { InitializationKind.HeadSection, 1 },
                { InitializationKind.TailSection, 5 },
                { InitializationKind.Uninitialized, 0 },
            };

        private readonly DateTime _zeroTime = new DateTime(1970, 1, 1);

        public MsulMessage GetMessage(MsulEmulationParametersViewModel ViewModel, InitializationKind InitializationKind)
        {
            return new MsulMessage(GetCommonData(ViewModel, InitializationKind),
                                   ViewModel.Carriages.Select(GetCarriageData).ToList());
        }

        private byte[] GetCommonData(MsulEmulationParametersViewModel ViewModel, InitializationKind InitializationKind)
        {
            using (var ms = new MemoryStream())
            {
                var writer = new BinaryWriter(ms);

                writer.Write((Byte)1);
                writer.Write(_initializationKinds[InitializationKind]);
                writer.Write((Byte)ViewModel.Carriages.Count);
                writer.Write((UInt32)(ViewModel.Time - _zeroTime).TotalSeconds);
                writer.Write((UInt16)ViewModel.TrainNumber);
                writer.Write((Byte)ViewModel.Speed);
                writer.Write(EncodeTemperature(ViewModel.OutdoorTemperature));
                writer.Write((Int32)(ViewModel.Position.Longitude.Value / LatitudeFactor));
                writer.Write((Int32)(ViewModel.Position.Latitude.Value / LatitudeFactor));
                writer.Write((Int16)ViewModel.Altitude);
                writer.Write((Byte)10);
                writer.Write((Byte)((ViewModel.EmergencyStop ? 1 : 0) << 0 |
                                    (ViewModel.LeftDoorLocked ? 0 : 1) << 1 |
                                    (ViewModel.RightDoorLocked ? 0 : 1) << 2 |
                                    (ViewModel.LeftDoorOpened ? 1 : 0) << 3 |
                                    (ViewModel.RightDoorOpened ? 1 : 0) << 4 |
                                    (ViewModel.LightOn ? 1 : 0) << 5));
                writer.Write((Byte)1);
                return ms.ToArray();
            }
        }

        private byte[] GetCarriageData(CarriageParametersViewModel carriage)
        {
            using (var ms = new MemoryStream())
            {
                var writer = new BinaryWriter(ms);
                writer.Write((Byte)carriage.Number);
                writer.Write(_carriageKinds[carriage.Kind]);
                writer.Write(EncodeTemperature(carriage.IndoorTemperature));
                writer.Write((Byte)((carriage.EmergencyValueReleased ? 1 : 0) << 0 |
                                    (carriage.Toilet1Occupied ? 1 : 0) << 1 |
                                    (carriage.Toilet2Occupied ? 1 : 0) << 2));
                writer.Write((Byte)0);
                writer.Write((Byte)0);
                return ms.ToArray();
            }
        }

        private byte EncodeTemperature(double TemperatureValue) { return (byte)Math.Round(TemperatureValue + 60); }
    }
}
