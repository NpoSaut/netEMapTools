using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using BlokFrames;
using Communications.Can;

namespace EMapNavigator.Emulation
{
    public interface IWheel {
        Double Milage { get; }
        Double Speed { get; set; }
        event EventHandler MilageChanged;
    }

    public class CanWheel : IDisposable, IWheel
    {
        public CanPort Port { get; private set; }

        public Double Milage { get; set; }
        public event EventHandler MilageChanged;
        protected virtual void OnMilageChanged()
        {
            var handler = MilageChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        private readonly Timer _pumpTimer;

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

        private void ListenForIpdState(object Obj)
        {
            var flow = (CanFlow)Obj;
            double oldOrdinate = Double.NaN;
            while (true)
            {
                var f = flow.Read().First();
                var stateFrame = BlokFrame.GetBlokFrame<IpdState>(f);
                //Debug.Print("ORD: {0}  | SPEED: {1}   | IMP: {2} | {3}", stateFrame.LinearOrdinate, stateFrame.Speed, stateFrame.SpeedPulsesAvailable, f);
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
            var f = GetPumpingFrame();
            Port.Send(f);
        }

        public Double Speed { get; set; }

        private int? CogsCount { get; set; }
        private Double? BondageDiameter { get; set; }

        private int xxx = -1;
        private IpdEmulation GetPumpingFrame()
        {
            xxx = -xxx;
            return new IpdEmulation
                   {
                       Sensor1State = IpdEmulation.SensorState.Get(-Speed * 160, CogsCount ?? 42, BondageDiameter ?? 1200, IpdEmulation.SensorState.DpsSensorPlacement.Left),
                       Sensor2State = IpdEmulation.SensorState.Get(-Speed * 160, CogsCount ?? 42, BondageDiameter ?? 1200, IpdEmulation.SensorState.DpsSensorPlacement.Left)
                   };
        }

        private void GetParameters()
        {
            //Port.Send(new SysDataQuery());
            CogsCount = 300;
            BondageDiameter = 714;
        }

        public void Dispose() { _pumpTimer.Dispose(); }
    }
}