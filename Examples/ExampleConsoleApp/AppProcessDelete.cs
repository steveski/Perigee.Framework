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
    using Perigee.Framework.Base.Database;
    using Perigee.Framework.EntityFramework;

    public class AppProcessDelete
    {
        private readonly IProcessQueries _processQueries;
        private readonly IProcessCommands _processCommands;

        public AppProcessDelete(IProcessQueries processQueries, IProcessCommands processCommands)
        {
            _processQueries = processQueries;
            _processCommands = processCommands;
        }

        public async Task Run()
        {
            var tokenSource = new CancellationTokenSource();
         
            
            
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
            
            await _processCommands.Execute(addCustomerCommand1, tokenSource.Token).ConfigureAwait(false);
            await _processCommands.Execute(addCustomerCommand2, tokenSource.Token).ConfigureAwait(false);

            //
            Console.WriteLine("Querying All");
            var customersByQueryAll = new CustomersBy();
            var results = await _processQueries.Execute(customersByQueryAll, tokenSource.Token).ConfigureAwait(false);
            var resultsList = results?.ToList();

            if (resultsList != null && resultsList.Count > 0)
            {
                var json = JsonSerializer.Serialize(resultsList, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                Console.WriteLine("Query result:");
                Console.WriteLine(json);
            }

            //
            Console.WriteLine("Deleting Bob");
            var deleteCommand = new DeleteCustomerCommand() {Id = addCustomerCommand1.CreatedEntity.Id};
            await _processCommands.Execute(deleteCommand, tokenSource.Token).ConfigureAwait(false);


            //
            Console.WriteLine("Querying All Again");
            results = await _processQueries.Execute(customersByQueryAll, tokenSource.Token).ConfigureAwait(false);
            resultsList = results?.ToList();
            
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
