using System;
using Geographics;

namespace Tracking
{
    public interface IPathRider
    {
        EarthPoint PointAt(Double Offset);
    }
}
