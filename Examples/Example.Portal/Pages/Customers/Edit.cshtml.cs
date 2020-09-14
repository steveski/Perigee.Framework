using Example.Portal.Entities;
using Example.Portal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace Example.Portal.Pages.Customers
{
    public class EditModel : PageModel
    {
        private readonly ICustomerDataService _customerDataService;

        [BindProperty]
        public Customer customer { get; set; }

        public EditModel(ICustomerDataService customerDataService)
        {
            _customerDataService = customerDataService;
        }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
                return NotFound();

            if (id.HasValue)
            {
                customer = await _customerDataService.getCustomer(id.Value);

                if (customer == null)
                    return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _customerDataService.updateCustomerName(customer.Id, customer.customerName);

            return RedirectToPage("./Index");
        }
    }
}