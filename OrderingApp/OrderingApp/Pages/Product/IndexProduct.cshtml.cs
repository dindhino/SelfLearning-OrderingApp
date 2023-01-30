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
    public class IndexProductModel : PageModel
    {
        private readonly OrderingApp.OrderingAppDataContext _context;

        public IndexProductModel(OrderingApp.OrderingAppDataContext context)
        {
            _context = context;
        }

        public IList<OrderingApp.EntityClasses.Product> Products { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Products != null)
            {
                Products = await _context.Products.ToListAsync();
            }
        }
    }
}
