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
    public class DetailsProductModel : PageModel
    {
        private readonly OrderingApp.OrderingAppDataContext _context;

        public DetailsProductModel(OrderingApp.OrderingAppDataContext context)
        {
            _context = context;
        }

      public OrderingApp.EntityClasses.Product Product { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            else 
            {
                Product = product;
            }
            return Page();
        }
    }
}
