using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace OrderingApp.Pages
{
    [Authorize]
    public class IndexCustomerModel : PageModel
    {
        private readonly OrderingApp.OrderingAppDataContext _context;

        public IndexCustomerModel(OrderingApp.OrderingAppDataContext context)
        {
            _context = context;
        }

        public IList<OrderingApp.EntityClasses.Customer> Customers { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Customers != null)
            {
                Customers = await _context.Customers.ToListAsync();
            }
        }
    }
}
