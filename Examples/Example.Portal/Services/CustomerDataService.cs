using Example.Portal.Domain.Customer.Commands;
using Example.Portal.Domain.Customer.Queries;
using Perigee.Framework.Base.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Example.Portal.Services
{
    public interface ICustomerDataService : IExamplePortalDataService
    {
        public Task<IEnumerable<Entities.Customer>> getCustomers();
        public Task<Entities.Customer> getCustomer(Guid id);
        public Task<Entities.Customer> addCustomer(string customerName);
        public Task updateCustomerName(Guid id, string newCustomerName);
    }

    public class CustomerDataService : ICustomerDataService
    {
        private IProcessCommands _processCommands { get; set; }
        private IProcessQueries _processQueries { get; set; }

        public CustomerDataService(IProcessCommands processCommands, IProcessQueries processQueries)
        {
            _processCommands = processCommands;
            _processQueries = processQueries;
        }

        public async Task<IEnumerable<Entities.Customer>> getCustomers()
        {
            var query = new CustomerQuery();
            var data = await _processQueries.Execute(query, CancellationToken.None);
            return data;
        }

        public async Task<Entities.Customer> getCustomer(Guid id)
        {
            var query = new CustomerQuery
            {
                id = id
            };
            var data = await _processQueries.Execute(query, CancellationToken.None);
            return data.FirstOrDefault();
        }

        public async Task<Entities.Customer> addCustomer(string customerName)
        {
            var cmd = new AddCustomerCommand
            {
                customerName = customerName
            };

            await _processCommands.Execute(cmd, CancellationToken.None);

            return cmd.CreatedEntity;
        }

        public async Task updateCustomerName(Guid id, string newCustomerName)
        {
            var cmd = new UpdateCustomerNameCommand
            {
                id = id,
                customerName = newCustomerName
            };

            await _processCommands.Execute(cmd, CancellationToken.None);
        }
    }
}
