using System;

namespace Tracking.Formatters.CanLog
{
    public struct MmAltLongFrame
    {
        private MmAltLongFrame(double Latitude, double Longitude, bool IsReliable = true)
        {
            this.Latitude  = Latitude;
            this.Longitude = Longitude;
            Reliable       = IsReliable;
        }

        public static ushort Descriptor => 0x4268;

        /// <summary>Достоверность данных</summary>
        public bool Reliable { get; set; }

        /// <summary>Широта (град.)</summary>
        /// <remarks>По Y</remarks>
        public double Latitude { get; set; }

        /// <summary>Долгота (град.)</summary>
        /// <remarks>По X</remarks>
        public double Longitude { get; set; }

        public static MmAltLongFrame Decode(byte[] Data)
        {
            var intLatitude  = BitConverter.ToInt32(Data, 0);
            var intLongitude = BitConverter.ToInt32(Data, 4) & ~(1 << 31);

            return new MmAltLongFrame(
                intLatitude  * 180.0 / (1e8 * Math.PI),
                intLongitude * 180.0 / (1e8 * Math.PI),
                (Data[7] & (1 << 7)) == 0);
        }

        #region Перевод в радианы

        /// <summary>Широта (в радианах)</summary>
        /// <remarks>По Y</remarks>
        public double LatitudeRad
        {
            get => Latitude                 * Math.PI / 180.0;
            set => Latitude = value * 180.0 / Math.PI;
        }

        /// <summary>Долгота (в радианах)</summary>
        /// <remarks>По X</remarks>
        public double LongitudeRad
        {
            get => Longitude                 * Math.PI / 180.0;
            set => Longitude = value * 180.0 / Math.PI;
        }

        #endregion
    }
}