namespace ExampleConsoleApp
{
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = ContainerConfiguration.Configure();
            var theProcess = serviceProvider.GetService(typeof(AppProcess)) as AppProcess;

            if (theProcess == null) 
                Console.WriteLine("AppProcess registration failed");
                
            theProcess.Run().Wait();

        }
    }
}
