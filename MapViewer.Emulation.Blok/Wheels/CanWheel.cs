using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Timers;
using BlokFrames;
using Communications.Can;
using MapViewer.Emulation.Wheels;

namespace MapViewer.Emulation.Blok.Wheels
{
    public class CanWheel : IDisposable, IWheel
    {
        private readonly Subject<double> _milage;
        private readonly Timer _pumpTimer;
        private double _milageValue;
        private int xxx = -1;

        public CanWheel(CanPort Port)
        {
            this.Port = Port;
            GetParameters();

            ICanFlow inputFlow = new CanFlow(Port, BlokFrame.GetDescriptors<IpdState>()[HalfsetKind.SetA]);
            Task.Factory.StartNew(ListenForIpdState, inputFlow);

            _pumpTimer = new Timer(100);
            _pumpTimer.Elapsed += PumpTimerOnElapsed;
            _pumpTimer.Start();

            _milageValue = 0;
            _milage = new Subject<double>();
        }

        public CanPort Port { get; private set; }
        private int? CogsCount { get; set; }
        private Double? BondageDiameter { get; set; }
        public void Dispose() { _pumpTimer.Dispose(); }

        public IObservable<double> Milage
        {
            get { return _milage; }
        }

        public Double Speed { get; set; }

        private void ListenForIpdState(object Obj)
        {
            var flow = (CanFlow)Obj;
            double oldOrdinate = Double.NaN;
            while (true)
            {
                CanFrame f = flow.Read().First();
                var stateFrame = BlokFrame.GetBlokFrame<IpdState>(f);
                Debug.Print("ORD: {0}  | SPEED: {1}   | IMP: {2} | {3}", stateFrame.LinearOrdinate, stateFrame.Speed, stateFrame.SpeedPulsesAvailable, f);
                if (!Double.IsNaN(oldOrdinate) && Math.Abs(stateFrame.LinearOrdinate - oldOrdinate) < 500)
                {
                    _milageValue += stateFrame.LinearOrdinate - oldOrdinate;
                    _milage.OnNext(_milageValue);
                }
                oldOrdinate = stateFrame.LinearOrdinate;
            }
        }

        private void PumpTimerOnElapsed(object Sender, ElapsedEventArgs Args)
        {
            IpdEmulation f = GetPumpingFrame();
            Port.Send(f);
        }

        private IpdEmulation GetPumpingFrame()
        {
            xxx = -xxx;
            return new IpdEmulation
                   {
                       Sensor1State =
                           IpdEmulation.SensorState.Get(-Speed * 160, CogsCount ?? 42, BondageDiameter ?? 1200, IpdEmulation.SensorState.DpsSensorPlacement.Left),
                       Sensor2State =
                           IpdEmulation.SensorState.Get(-Speed * 160, CogsCount ?? 42, BondageDiameter ?? 1200, IpdEmulation.SensorState.DpsSensorPlacement.Left)
                   };
        }

        private void GetParameters()
        {
            //Port.Send(new SysDataQuery());
            CogsCount = 300;
            BondageDiameter = 714;
        }
    }
}
