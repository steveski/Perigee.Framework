namespace ExampleConsoleApp
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json;
    using System.Threading;
    using Perigee.Framework.Base.Transactions;
    using System.Threading.Tasks;
    using Autofac;
    using Example.Domain.Customers.Commands;
    using Example.Domain.Customers.Queries;
    using Example.Domain.Customers.Views;
    using Perigee.Framework.Base.Database;
    using Perigee.Framework.EntityFramework;

    public class AppProcess
    {
        private readonly IProcessQueries _processQueries;
        private readonly IProcessCommands _processCommands;

        public AppProcess(IProcessQueries processQueries, IProcessCommands processCommands)
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


            //await Task.WhenAll(new[]
            //{
            //    _processCommands.Execute(addCustomerCommand1, tokenSource.Token),
            //    _processCommands.Execute(addCustomerCommand2, tokenSource.Token)

            //}).ConfigureAwait(false);
            
            await _processCommands.Execute(addCustomerCommand1, tokenSource.Token).ConfigureAwait(false);
            await _processCommands.Execute(addCustomerCommand2, tokenSource.Token).ConfigureAwait(false);

            var customersByQuery = new CustomersBy
            {
                FirstName = "Bob"
            };

            var resultsList = new List<GetCustomerWithAddressView>();

            // Query using "Projector" pattern where the mapping is done on Sql Server rather than in this application.
            // This approach is more efficient as only the required data ever comes back to the application
            var results = await _processQueries.Execute(customersByQuery, tokenSource.Token).ConfigureAwait(false);
            if(results != null)
                resultsList.AddRange(results.Where(x => x.Address != null));

            // Query using Include expressions. There may be scenarios where an include is required. EF Core poses restrictions
            // when using AsExpandable() from LinqKit, so this QueryHandler shows how to pass Include expressions to Query<TEntity>(...)
            var customersWithIncludeByQuery = new CustomersWithIncludeBy()
            {
                FirstName = "Bob"
            };

            var resultsUsingInclude = await _processQueries.Execute(customersWithIncludeByQuery, tokenSource.Token).ConfigureAwait(false);
            if(resultsUsingInclude != null)
                resultsList.AddRange(resultsUsingInclude.Where(x => x.Address != null));


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
