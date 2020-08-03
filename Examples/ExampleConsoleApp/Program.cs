namespace ExampleConsoleApp
{
    using System;
    using System.Security.Claims;
    using System.Security.Principal;
    using Microsoft.Extensions.Configuration;

    class Program
    {
        static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

            var databaseConfig = configuration.GetSection("Database").Get<DatabaseConfig>();
            var aesConfig = configuration.GetSection("Aes").Get<AesConfig>();

            var userIdentity = new GenericIdentity(Environment.UserDomainName + "\\" + Environment.UserName, "Anonymous");
            
            var principal = new ClaimsPrincipal(userIdentity);
            AppDomain.CurrentDomain.SetThreadPrincipal(principal);


            var serviceProvider = ContainerConfiguration.Configure(principal, databaseConfig, aesConfig);
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
