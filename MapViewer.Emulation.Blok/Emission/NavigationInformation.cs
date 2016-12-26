using Geographics;

namespace MapViewer.Emulation.Blok.Emission
{
    public class NavigationInformation
    {
        public NavigationInformation(EarthPoint Position, double Speed, bool Reliability)
        {
            this.Reliability = Reliability;
            this.Position = Position;
            this.Speed = Speed;
        }

        public EarthPoint Position { get; private set; }
        public double Speed { get; private set; }
        public bool Reliability { get; private set; }
    }
}