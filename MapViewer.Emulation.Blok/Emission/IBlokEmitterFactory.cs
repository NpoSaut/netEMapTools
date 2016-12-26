namespace MapViewer.Emulation.Blok.Emission
{
    public interface IBlokEmitterFactory
    {
        string Name { get; }
        IBlokEmitter CreatEmitter();
    }
}
