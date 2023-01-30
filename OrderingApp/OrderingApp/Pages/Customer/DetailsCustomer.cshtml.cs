using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OrderingApp.Data;

namespace OrderingApp.Pages
{
    [Authorize]
    public class DetailsCustomerModel : PageModel
    {
        private readonly OrderingApp.OrderingAppDataContext _context;

        public DetailsCustomerModel(OrderingApp.OrderingAppDataContext context)
        {
            _context = context;
        }

      public OrderingApp.EntityClasses.Customer Customer { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }
            else 
            {
                Customer = customer;
            }
            return Page();
        }
    }
}
