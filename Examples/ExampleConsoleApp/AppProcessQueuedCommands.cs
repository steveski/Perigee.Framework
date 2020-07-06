namespace ExampleConsoleApp
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Text.Json;
    using System.Threading;
    using Perigee.Framework.Base.Transactions;
    using System.Threading.Tasks;
    using Autofac;
    using Example.Domain.Customers.Commands;
    using Example.Domain.Customers.Queries;
    using Example.Services;
    using Perigee.Framework.Base.Database;
    using Perigee.Framework.EntityFramework;

    public class AppProcessQueuedCommands
    {
        private readonly IProcessQueries _processQueries;
        private readonly ICommandProcessorQueue _commandProcessorQueue;

        public AppProcessQueuedCommands(IProcessQueries processQueries, ICommandProcessorQueue commandProcessorQueue)
        {
            _processQueries = processQueries;
            _commandProcessorQueue = commandProcessorQueue;
        }

        public async Task Run()
        {
            Console.WriteLine("Queued command processor");

            var tokenSource = new CancellationTokenSource();

            var commandQueueTask = _commandProcessorQueue.StartProcessing(tokenSource.Token);


            var addCustomerCommand1 = new CreateCustomerCommand
            {
                FirstName = "Bob",
                LastName = "Jones",
                EmailAddress = "bob.jones@home.com",
                //Commit = false
            };

            var addCustomerCommand2 = new CreateCustomerCommand
            {
                FirstName = "Herbert",
                LastName = "Scrackle",
                EmailAddress = "herbert.scrackle@home.com",
                //Commit =  false
            };


            _commandProcessorQueue.EnqueueCommand(addCustomerCommand1, tokenSource.Token);
            _commandProcessorQueue.EnqueueCommand(addCustomerCommand2, tokenSource.Token);
            
            _commandProcessorQueue.FinaliseQueue();

            await commandQueueTask.ConfigureAwait(false);

            
            var customersByQuery = new CustomersBy
            {
                FirstName = "Herbert"
            };

            var results = await _processQueries.Execute(customersByQuery, tokenSource.Token).ConfigureAwait(false);
            var resultsList = results?.ToList();

            Console.WriteLine("Records created: 2");
            Console.WriteLine($"Querying after creation for Customer with First Name of 'Herbert' returns {resultsList?.Count ?? 0}");

            if (resultsList != null && resultsList.Count > 0)
            {
                var json = JsonSerializer.Serialize(resultsList, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                Console.WriteLine("Query result:");
                Console.WriteLine(json);
            }

        }


    }
}
