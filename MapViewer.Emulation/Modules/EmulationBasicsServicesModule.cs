using MapViewer.Emulation.Wheels;
using Microsoft.Practices.Unity;

namespace MapViewer.Emulation.Modules
{
    public class EmulationBasicsServicesModule : UnityContainerModule
    {
        public EmulationBasicsServicesModule(IUnityContainer Container) : base(Container) { }

        public override void Initialize()
        {
            Container
                .RegisterType<IWheel, VirtualWheel>(new ContainerControlledLifetimeManager());
        }
    }
}