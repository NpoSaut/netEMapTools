using MapViewer;
using Microsoft.Practices.Unity;
using MsulEmulation.Emit;
using MsulEmulation.Encoding;

namespace MsulEmulation.Modules
{
    public class MsulEmulationServicesModule : UnityContainerModule
    {
        public MsulEmulationServicesModule(IUnityContainer Container) : base(Container) { }

        public override void Initialize()
        {
            Container
                .RegisterType<IMsulMessageEncoder, MsulMessageEncoder>(new ContainerControlledLifetimeManager())
                .RegisterType<IMsulEmitter, UdpMsulEmitter>(new ContainerControlledLifetimeManager());
        }
    }
}