using Example.Portal.Domain.Customer.Commands;
using Example.Portal.Domain.Customer.Queries;
using Perigee.Framework.Base.Transactions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Example.Portal.Services
{
    public interface ICustomerDataService : IExamplePortalDataService
    {
        public Task<IEnumerable<Entities.Customer>> getCustomers();
        public Task<Entities.Customer> addCustomer(string customerName);
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

        public async Task<Entities.Customer> addCustomer(string customerName)
        {
            var cmd = new AddCustomerCommand
            {
                customerName = customerName
            };

            await _processCommands.Execute(cmd, CancellationToken.None);

            return cmd.CreatedEntity;
        }
    }
}
