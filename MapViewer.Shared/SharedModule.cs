using System;
using Microsoft.Practices.Unity;

namespace MapViewer
{
    public class SharedModule : UnityContainerModule
    {
        public SharedModule(IUnityContainer Container) : base(Container) { }

        public override void Initialize() { throw new NotImplementedException(); }
    }
}
