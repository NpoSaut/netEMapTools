using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Timers;
using BlokFrames;
using Communications.Can;
using Communications.PortHelpers;
using MapViewer.Emulation.Wheels;

namespace MapViewer.Emulation.Blok.Wheels
{
    public class CanWheel : IDisposable, IWheel
    {
        private readonly Subject<double> _milage;
        private readonly Timer _pumpTimer;
        private double _milageValue;
        private double _oldOrdinate;

        public CanWheel(ICanPort Port)
        {
            this.Port = Port;
            GetParameters();

            var descriptor = BlokFrame.GetDescriptors<IpdState>()[HalfsetKind.SetA];
            Port.Rx.WaitForTransactionCompleated()
                .Where(f => f.Descriptor == descriptor)
                .Select(BlokFrame.GetBlokFrame<IpdState>)
                .Subscribe(ProcessIpdState);

            _pumpTimer = new Timer(100);
            _pumpTimer.Elapsed += PumpTimerOnElapsed;
            _pumpTimer.Start();

            _milageValue = 0;
            _milage = new Subject<double>();
        }

        public ICanPort Port { get; private set; }
        private int? CogsCount { get; set; }
        private Double? BondageDiameter { get; set; }
        public void Dispose() { _pumpTimer.Dispose(); }

        public event EventHandler SpeedChanged;

        public IObservable<double> Milage
        {
            get { return _milage; }
        }

        public void Reset() { throw new NotImplementedException(); }

        public Double Speed { get; set; }

        private void ProcessIpdState(IpdState IpdState)
        {
            Debug.Print("ORD: {0}  | SPEED: {1}   | IMP: {2}", IpdState.LinearOrdinate, IpdState.Speed, IpdState.SpeedPulsesAvailable);
            if (!Double.IsNaN(_oldOrdinate) && Math.Abs(IpdState.LinearOrdinate - _oldOrdinate) < 500)
            {
                _milageValue += IpdState.LinearOrdinate - _oldOrdinate;
                _milage.OnNext(_milageValue);
            }
            _oldOrdinate = IpdState.LinearOrdinate;
        }

        private void PumpTimerOnElapsed(object Sender, ElapsedEventArgs Args)
        {
            IpdEmulation f = GetPumpingFrame();
            Port.BeginSend(f);
        }

        private IpdEmulation GetPumpingFrame()
        {
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
