namespace Example.Services
{
    using Autofac;
    using Perigee.Framework.Base.Database;

    public class ExampleServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CommandProcessorQueue>().As<ICommandProcessorQueue>().SingleInstance();

            //builder.RegisterType<GeneralUserRecordAuthority>().As<IRecordAuthority>().InstancePerLifetimeScope();
            builder.RegisterType<AllAuthorisedRecordAuthority>().As<IRecordAuthority>().InstancePerLifetimeScope();

        }

    }

}
