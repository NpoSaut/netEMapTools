using System;
using Microsoft.Practices.Unity;

namespace MapViewer.InterfaceElements
{
    public static class InterfaceElementsRegistrationHelper
    {
        public static IUnityContainer RegisterCommandGroup<TToolbarCommandGroup>(this IUnityContainer Container,
                                                                                 Func<IUnityContainer, TToolbarCommandGroup> ToolbarCommandGroupFactory)
            where TToolbarCommandGroup : IToolbarCommandGroup
        {
            return Container.RegisterType<IToolbarCommandGroup, TToolbarCommandGroup>(
                Guid.NewGuid().ToString(),
                new ContainerControlledLifetimeManager(),
                new InjectionFactory(c => ToolbarCommandGroupFactory(c)));
        }
    }
}
