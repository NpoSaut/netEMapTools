using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using BlokFrames;
using Communications.Can;

namespace EMapNavigator.Emulation
{
    public class CanWheel : IDisposable, IWheel
    {
        private readonly Timer _pumpTimer;
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
        }

        public CanPort Port { get; private set; }
        private int? CogsCount { get; set; }
        private Double? BondageDiameter { get; set; }
        public void Dispose() { _pumpTimer.Dispose(); }

        public Double Milage { get; set; }
        public event EventHandler MilageChanged;
        public Double Speed { get; set; }

        protected virtual void OnMilageChanged()
        {
            EventHandler handler = MilageChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }

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
                    Milage += stateFrame.LinearOrdinate - oldOrdinate;
                    OnMilageChanged();
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
