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
    public class IndexOrderModel : PageModel
    {
        private readonly OrderingApp.OrderingAppDataContext _context;

        public IndexOrderModel(OrderingApp.OrderingAppDataContext context)
        {
            _context = context;
        }

        public IList<OrderingApp.EntityClasses.Order> Orders { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Orders != null)
            {
                Orders = await _context.Orders.ToListAsync();
            }

            foreach(var order in Orders)
            {
                order.Customer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == order.CustomerId);
                order.OrderItems = await _context.OrderItems.Where(x => x.OrderId == order.Id).ToListAsync();
                foreach(var orderItem in order.OrderItems)
                {
                    orderItem.Product = await _context.Products.FirstOrDefaultAsync(x => x.Id == orderItem.ProductId);
                }

            }
        }
    }
}
