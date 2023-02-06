using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using OrderingApp.EntityClasses;
using System.Threading;
using System;
using System.Net;
using System.Composition;
using static NuGet.Packaging.PackagingConstants;

namespace OrderingApp.Controller
{
    [Authorize]
    public class OrderController
    {
        private readonly string NotFound = "Not Found";
        private readonly OrderingAppDataContext _context;

        public OrderController(OrderingAppDataContext context) => _context = context;

        [HttpGet]
        public async Task<IList<Order>> GetAllOrderList()
        {
            try
            {
                var orders = await _context.Orders.ToListAsync();
                await GetOrderDetails(orders);

                return orders;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task GetOrderDetails(List<Order> orders)
        {
            foreach (var order in orders)
            {
                await JustGetOrderDetails(order);
            }
        }

        private async Task JustGetOrderDetails(Order order)
        {
            order.Customer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == order.CustomerId);
            order.Customer.Orders.Clear();

            order.OrderItems = await _context.OrderItems.Where(x => x.OrderId == order.Id).ToListAsync();
            foreach (var orderItem in order.OrderItems)
            {
                orderItem.Product = await _context.Products.FirstOrDefaultAsync(x => x.Id == orderItem.ProductId);
                orderItem.Product.OrderItems.Clear();

                orderItem.Order = null;
            }
        }

        [HttpGet]
        public async Task<Order> GetOrder(int? id)
        {
            try
            {
                if (id == null || _context.Orders == null)
                    throw new Exception(NotFound);
                Order? order = await JustGetOrder(id);
                JustGetOrderDetails(order);
                return order;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<Order> JustGetOrder(int? id)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
                throw new Exception(NotFound);
            return order;
        }

        //[HttpPost]
        //public async Task<Order> CreateOrder([FromBody] Order order)
        //{
        //    try
        //    {
        //        JustValidate(order);

        //        _context.Orders.Add(order);
        //        await _context.SaveChangesAsync();

        //        return order;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //private void JustValidate(Order order)
        //{
        //    //throw new NotImplementedException();
        //}

        //[HttpPut]
        //public async Task<Order> UpdateOrder([FromBody] Order order, int? id)
        //{
        //    try
        //    {
        //        JustValidate(order);

        //        if (_context.Orders == null)
        //            throw new Exception(NotFound);

        //        Order? existingOrder = await JustGetOrder(id);
        //        existingOrder.Name = order.Name;
        //        existingOrder.Description = order.Description;
        //        order = existingOrder;
        //        _context.Attach(order).State = EntityState.Modified;

        //        try
        //        {
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!OrderExists(order.Id))
        //            {
        //                throw new Exception(NotFound);
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }

        //        return order;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //private bool OrderExists(int id)
        //{
        //    return _context.Orders.Any(e => e.Id == id);
        //}

        //[HttpDelete]
        //public async Task<Order> DeleteOrder(int? id)
        //{
        //    try
        //    {
        //        if (id == null || _context.Orders == null)
        //        {
        //            throw new Exception(NotFound);
        //        }
        //        Order? order = await JustGetOrder(id);
        //        _context.Orders.Remove(order);
        //        await _context.SaveChangesAsync();

        //        return order;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}
