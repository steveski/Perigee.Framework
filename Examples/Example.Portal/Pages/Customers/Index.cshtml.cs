using Example.Portal.Entities;
using Example.Portal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Example.Portal.Pages.Customers
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly ICustomerDataService _customerDataService;
        public List<Customer> customers { get; set; }

        public IndexModel(ICustomerDataService customerDataService)
        {
            _customerDataService = customerDataService;
        }

        public async System.Threading.Tasks.Task OnGetAsync()
        {
            customers = (await _customerDataService.getCustomers()).ToList();
        }
    }
}
