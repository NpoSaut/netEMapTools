using System.Collections.Generic;
using System.Linq;
using MapViewer.Emulation.Blok.Emission.Options;
using ReactiveUI;

namespace MapViewer.Emulation.Blok.ViewModels.Options
{
    public class DescriptorSelectorViewModel : ReactiveObject, IDescriptorEmissionOption
    {
        private EmissionDescriptorViewModel _selectedDescriptor;

        public DescriptorSelectorViewModel()
        {
            Descriptors = new List<EmissionDescriptorViewModel>
                          {
                              new EmissionDescriptorViewModel("Эмуляция", 0xb808),
                              new EmissionDescriptorViewModel("ЭК-СНС", 0x4268)
                          };
            _selectedDescriptor = Descriptors.First();
        }

        public IList<EmissionDescriptorViewModel> Descriptors { get; private set; }

        public EmissionDescriptorViewModel SelectedDescriptor
        {
            get { return _selectedDescriptor; }
            set { this.RaiseAndSetIfChanged(ref _selectedDescriptor, value); }
        }

        int IDescriptorEmissionOption.EmissionDescriptor
        {
            get { return SelectedDescriptor.Descriptor; }
        }
    }
}
