﻿namespace ExampleConsoleApp
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


            //await Task.WhenAll(new[]
            //{
            //    _processCommands.Execute(addCustomerCommand1, tokenSource.Token),
            //    _processCommands.Execute(addCustomerCommand2, tokenSource.Token)

            //}).ConfigureAwait(false);

            var customersByQuery = new CustomersBy
            {
                FirstName = "Herbert"
            };

            var results = await _processQueries.Execute(customersByQuery, tokenSource.Token).ConfigureAwait(false);
            var resultsList = results?.ToList();

            Console.WriteLine("Records created: 2 addresses, 3 people");
            Console.WriteLine($"Querying for Customer with First Name of 'Herbert' returns {resultsList?.Count ?? 0}");

            if (resultsList != null && resultsList.Count > 0)
            {
                var json = JsonSerializer.Serialize(resultsList, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                Console.WriteLine("Query result:");
                Console.WriteLine(json);
            }

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

            // Get all address
            addressesByQuery = new AddressesBy
            {
            };

            addressResults = await _processQueries.Execute(addressesByQuery, tokenSource.Token).ConfigureAwait(false);
            addressResultsList = addressResults?.ToList();
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

            // Now lets look at the detailed results (Customer with Address)
            var customersWithAddressByQuery = new CustomersWithAddressBy
            {
                FirstName = "Herbert"
            };

            var detailedResults = await _processQueries.Execute(customersWithAddressByQuery, tokenSource.Token).ConfigureAwait(false);
            var detailedResultsList = detailedResults?.ToList();

            Console.WriteLine($"Querying full details for Customer with First Name of 'Herbert' returns {detailedResultsList?.Count ?? 0}");

            if (detailedResultsList != null && detailedResultsList.Count > 0)
            {
                var json = JsonSerializer.Serialize(detailedResultsList, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                Console.WriteLine("Query result:");
                Console.WriteLine(json);
            }
        }
    }
}
