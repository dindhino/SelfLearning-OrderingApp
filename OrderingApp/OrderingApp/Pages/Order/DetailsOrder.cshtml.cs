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
    public class DetailsOrderModel : PageModel
    {
        private readonly OrderingApp.OrderingAppDataContext _context;

        public DetailsOrderModel(OrderingApp.OrderingAppDataContext context)
        {
            _context = context;
        }

        public OrderingApp.EntityClasses.Order Order { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            else
            {
                order.Customer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == order.CustomerId);
                order.OrderItems = await _context.OrderItems.Where(x => x.OrderId == order.Id).ToListAsync();
                foreach (var orderItem in order.OrderItems)
                    orderItem.Product = await _context.Products.FirstOrDefaultAsync(x => x.Id == orderItem.ProductId);

                Order = order;
            }
            return Page();
        }
    }
}
