using Autofac;
using System;

namespace Example.Portal.Services
{
    public class ExamplePortalServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            builder.RegisterAssemblyTypes(assemblies)
                .Where(t => t.IsAssignableTo<IExamplePortalDataService>())
                .AsImplementedInterfaces();
        }
    }
}
