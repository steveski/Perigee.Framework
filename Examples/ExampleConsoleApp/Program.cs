namespace ExampleConsoleApp
{
    using System;
    using System.Security.Claims;
    using System.Security.Principal;

    class Program
    {
        static void Main(string[] args)
        {
            var userIdentity = new GenericIdentity(Environment.UserDomainName + "\\" + Environment.UserName, "Anonymous");
            
            var principal = new ClaimsPrincipal(userIdentity);
            AppDomain.CurrentDomain.SetThreadPrincipal(principal);


            var serviceProvider = ContainerConfiguration.Configure(principal);
            var theProcess = serviceProvider.GetService(typeof(AppProcess)) as AppProcess;
            //var theProcess = serviceProvider.GetService(typeof(AppProcessQueuedCommands)) as AppProcessQueuedCommands;
            //var theProcess = serviceProvider.GetService(typeof(AppProcessDelete)) as AppProcessDelete;

            if (theProcess == null)
            {
                Console.WriteLine("AppProcess registration failed");
                return;
            }

            theProcess.Run().Wait();

        }
    }
}
