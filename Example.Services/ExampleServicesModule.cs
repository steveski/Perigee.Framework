namespace Example.Services
{
    using Autofac;

    public class ExampleServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CommandProcessorQueue>().As<ICommandProcessorQueue>().SingleInstance();

        }

    }

}
