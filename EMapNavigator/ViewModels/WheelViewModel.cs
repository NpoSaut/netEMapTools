using System;
using MapViewer.Emulation.Wheels;

namespace EMapNavigator.ViewModels
{
    public class WheelViewModel : ViewModelBase
    {
        public IWheel Wheel { get; private set; }

        private Double _speed;

        public Double Speed
        {
            get { return _speed; }
            set
            {
                if (_speed != value)
                {
                    _speed = value;
                    Wheel.Speed = Speed;
                    OnPropertyChanged("Speed");
                }
            }
        }

        private Double _milage;

        public WheelViewModel(IWheel Wheel)
        {
            this.Wheel = Wheel;
            this.Speed = Wheel.Speed;
            //this.Milage = Wheel.Milage;
            //Wheel.MilageChanged += (Sender, Args) => this.Milage = Wheel.Milage;
        }

        public Double Milage
        {
            get { return _milage; }
            private set
            {
                if (_milage != value)
                {
                    _milage = value;
                    OnPropertyChanged("Milage");
                }
            }
        }
    }
}