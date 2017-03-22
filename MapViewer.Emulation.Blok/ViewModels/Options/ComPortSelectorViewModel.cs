using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using ReactiveUI;

namespace MapViewer.Emulation.Blok.ViewModels.Options
{
    public class ComPortSelectorViewModel : ReactiveObject
    {
        private string _selectedPort;

        protected ComPortSelectorViewModel()
        {
            Ports = SerialPort.GetPortNames();
            _selectedPort = Ports.FirstOrDefault();
        }

        public IList<string> Ports { get; private set; }

        public string SelectedPort
        {
            get { return _selectedPort; }
            set { this.RaiseAndSetIfChanged(ref _selectedPort, value); }
        }
    }
}
