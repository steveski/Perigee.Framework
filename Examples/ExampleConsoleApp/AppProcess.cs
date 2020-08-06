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
    using Example.Domain.Addresses.Commands;
    using Example.Domain.Addresses.Queries;

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

            // Create 2 people at 1 address
            var addAddressCommand1 = new CreateAddressCommand
            {
                Street = "15 My Street",
                Suburb = "Springfield",
                State = "My State",
                Country = "Safe Country",
                PostalCode = "12345"
            };
            await _processCommands.Execute(addAddressCommand1, tokenSource.Token).ConfigureAwait(false);

            var addCustomerCommand1 = new CreateCustomerCommand
            {
                FirstName = "Bob",
                LastName = "Jones",
                EmailAddress = "bob.jones@home.com",
                AddressId = addAddressCommand1.CreatedEntity.Id
                //Commit = false
            };

            var addCustomerCommand2 = new CreateCustomerCommand
            {
                FirstName = "Herbert",
                LastName = "Scrackle",
                EmailAddress = "herbert.scrackle@home.com",
                AddressId = addAddressCommand1.CreatedEntity.Id
                //Commit =  false
            };

            await _processCommands.Execute(addCustomerCommand1, tokenSource.Token).ConfigureAwait(false);
            await _processCommands.Execute(addCustomerCommand2, tokenSource.Token).ConfigureAwait(false);

            // Now create 1 person at a second address
            var addAddressCommand2 = new CreateAddressCommand
            {
                Street = "742 Evergreen Terrace",
                Suburb = "Springfield",
                State = "Unknown",
                Country = "USA",
                PostalCode = "Unknown"
            };
            await _processCommands.Execute(addAddressCommand2, tokenSource.Token).ConfigureAwait(false);

            var addCustomerCommand3 = new CreateCustomerCommand
            {
                FirstName = "Homer",
                LastName = "Simpson",
                EmailAddress = "homer.simpson@the.power.plan",
                AddressId = addAddressCommand2.CreatedEntity.Id
                //Commit = false
            };
            await _processCommands.Execute(addCustomerCommand3, tokenSource.Token).ConfigureAwait(false);

            Console.WriteLine("Records created: 2 addresses, 3 people");

            // Now test getting data back
            {
                // Query using "Projector" pattern where the mapping is done on Sql Server rather than in this application.
                // This approach is more efficient as only the required data ever comes back to the application
                var customersByQuery = new CustomersBy
                {
                    FirstName = "Bob"
                };

                var results = await _processQueries.Execute(customersByQuery, tokenSource.Token).ConfigureAwait(false);
                var resultsList = results?.ToList();
                Console.WriteLine($"Querying for Customer with First Name of '{customersByQuery.FirstName}' returns {resultsList?.Count ?? 0}");
                if (resultsList != null && resultsList.Count > 0)
                {
                    var json = JsonSerializer.Serialize(resultsList, new JsonSerializerOptions
                    {
                        WriteIndented = true
                    });

                    Console.WriteLine("Query result:");
                    Console.WriteLine(json);
                }

                Console.WriteLine();
            }

            {
                // Query using Include expressions. There may be scenarios where an include is required. EF Core poses restrictions
                // when using AsExpandable() from LinqKit, so this QueryHandler shows how to pass Include expressions to Query<TEntity>(...)
                var customersWithIncludeByQuery = new CustomersWithIncludeBy()
                {
                    FirstName = "Bob"
                };

                var resultsUsingInclude = await _processQueries.Execute(customersWithIncludeByQuery, tokenSource.Token).ConfigureAwait(false);
                var resultsList = resultsUsingInclude?.ToList();

                Console.WriteLine($"Querying for Customer with First Name of '{customersWithIncludeByQuery.FirstName}' returns {resultsList?.Count ?? 0}");

                if (resultsList != null && resultsList.Count > 0)
                {
                    var json = JsonSerializer.Serialize(resultsList, new JsonSerializerOptions
                    {
                        WriteIndented = true
                    });

                    Console.WriteLine("Query result:");
                    Console.WriteLine(json);
                }

                Console.WriteLine();
            }

            {
                // Get a single address
                var addressesByQuery = new AddressesBy
                {
                    Id = addAddressCommand1.CreatedEntity.Id
                };

                var addressResults = await _processQueries.Execute(addressesByQuery, tokenSource.Token).ConfigureAwait(false);
                var addressResultsList = addressResults?.ToList();
                Console.WriteLine($"Querying for Address with id {addAddressCommand1.CreatedEntity.Id} returns {addressResultsList?.Count ?? 0}");

                if (addressResultsList != null && addressResultsList.Count > 0)
                {
                    var json = JsonSerializer.Serialize(addressResultsList, new JsonSerializerOptions
                    {
                        WriteIndented = true
                    });

                    Console.WriteLine("Query result (1 address):");
                    Console.WriteLine(json);
                }

                Console.WriteLine();
            }

            {
                // Get all address
                var addressesByQuery = new AddressesBy
                {
                };

                var addressResults = await _processQueries.Execute(addressesByQuery, tokenSource.Token).ConfigureAwait(false);
                var addressResultsList = addressResults?.ToList();
                Console.WriteLine($"Querying for all Addresses returns {addressResultsList?.Count ?? 0}");

                if (addressResultsList != null && addressResultsList.Count > 0)
                {
                    var json = JsonSerializer.Serialize(addressResultsList, new JsonSerializerOptions
                    {
                        WriteIndented = true
                    });

                    Console.WriteLine("Query result (multiple addresses):");
                    Console.WriteLine(json);
                }

                Console.WriteLine();
            }
        }
    }
}
