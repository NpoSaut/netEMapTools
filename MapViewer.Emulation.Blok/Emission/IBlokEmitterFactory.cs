namespace MapViewer.Emulation.Blok.Emission
{
    public interface IBlokEmitterFactory
    {
        string Name { get; }
        bool UsesDescriptor { get; }
        IBlokEmitter CreatEmitter(int Descriptor);
    }
}
