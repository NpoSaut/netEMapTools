using MapViewer.Emulation.Msul.Emit;
using MapViewer.Emulation.Msul.Encoding;
using Microsoft.Practices.Unity;

namespace MapViewer.Emulation.Msul.Modules
{
    public class MsulEmulationServicesModule : UnityContainerModule
    {
        public MsulEmulationServicesModule(IUnityContainer Container) : base(Container) { }

        public override void Initialize()
        {
            Container
                .RegisterType<IMsulMessageEncoder, MsulMessageEncoder>(new ContainerControlledLifetimeManager())
                .RegisterType<IMsulEmitterProvider, UdpMsulEmitterProvider>(new ContainerControlledLifetimeManager());
        }
    }
}
