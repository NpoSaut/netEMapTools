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
    public class CanWheel : IDisposable
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
                Debug.Print("ORD: {0}  | SPEED: {1}   | IMP: {2} | {3}", stateFrame.LinearOrdinate, stateFrame.Speed, stateFrame.SpeedPulsesAvailable, f);
                if (!Double.IsNaN(oldOrdinate) && Math.Abs(stateFrame.LinearOrdinate - oldOrdinate) < 500) 
                {
                    Milage += Math.Abs(stateFrame.LinearOrdinate - oldOrdinate);
                    OnMilageChanged();
                }
                oldOrdinate = stateFrame.LinearOrdinate;
            }
        }

        private void PumpTimerOnElapsed(object Sender, ElapsedEventArgs Args)
        {
            var f = GetPumpingFrame();
            //Console.WriteLine(f.GetCanFrame());
            Port.Send(f);
        }

        public Double Speed { get; set; }

        private Double? TeethCount { get; set; }
        private Double? BondageDiameter { get; set; }

        private IpdEmulation.SensorState GetSensorState(int multipler = 1)
        {
            return new IpdEmulation.SensorState
                   {
                       Channel1Condition = IpdEmulation.ChannelCondition.Good,
                       Channel2Condition = IpdEmulation.ChannelCondition.Good,
                       Direction = multipler * Speed > 0 ? IpdEmulation.RorationDirection.Clockwise : IpdEmulation.RorationDirection.Counterclockwise,
                       Frequncy = 160480 //(int)((Math.Abs(Speed) * 1000 * TeethCount) / (3.6 * Math.PI * BondageDiameter))
                   };
        }

        private int xxx = -1;
        private IpdEmulation GetPumpingFrame()
        {
            xxx = -xxx;
            return new IpdEmulation
                   {
                       Sensor1State = GetSensorState(0),
                       Sensor2State = GetSensorState(xxx)
                   };
        }

        private void GetParameters()
        {
            //Port.Send(new SysDataQuery());
            TeethCount = 300;
            BondageDiameter = 714;
        }

        public void Dispose() { _pumpTimer.Dispose(); }
    }
}