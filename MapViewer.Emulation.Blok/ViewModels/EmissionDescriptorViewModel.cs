namespace MapViewer.Emulation.Blok.ViewModels
{
    public class EmissionDescriptorViewModel
    {
        public EmissionDescriptorViewModel(string Name, int Descriptor)
        {
            this.Name = Name;
            this.Descriptor = Descriptor;
        }

        public string Name { get; private set; }
        public int Descriptor { get; private set; }
    }
}
