using Example.Portal.Entities;
using Example.Portal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Example.Portal.Pages.Customers
{
    public class CreateModel : PageModel
    {
        private readonly ICustomerDataService _customerDataService;

        [BindProperty]
        public Customer customer { get; set; }

        public CreateModel(ICustomerDataService customerDataService)
        {
            _customerDataService = customerDataService;
        }

        /*
        public void OnGet()
        {
            return Page();
        }
*/

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _customerDataService.addCustomer(customer.customerName);

            return RedirectToPage("./Index");
        }
    }
}