using System;
using System.Reactive.Linq;
using Geographics;
using Tracking;

namespace SimpleMsul.Model
{
    internal class FakePathRider : IPathRiderProvider
    {
        public IObservable<IPathRider> PathRider => Observable.Return(new Rider());

        private class Rider : IPathRider
        {
            public EarthPoint PointAt(double Offset)
            {
                return new EarthPoint(56.065138, 85.631024);
            }
        }
    }
}