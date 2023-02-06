using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using OrderingApp.EntityClasses;
using System.Threading;
using System;
using System.Net;
using System.Composition;

namespace OrderingApp.Controller
{
    [Authorize]
    public class CustomerController
    {
        private readonly string NotFound = "Not Found";
        private readonly OrderingAppDataContext _context;

        public CustomerController(OrderingAppDataContext context) => _context = context;

        [HttpGet]
        public async Task<IList<Customer>> GetAllCustomerList()
        {
            try
            {
                var customers = await _context.Customers.ToListAsync();
                customers.ForEach(x => x.Orders.Clear());
                return customers;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<Customer> GetCustomer(int? id)
        {
            try
            {
                if (id == null || _context.Customers == null)
                    throw new Exception(NotFound);
                Customer? customer = await JustGetCustomer(id);
                customer.Orders.Clear();
                return customer;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<Customer> JustGetCustomer(int? id)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
                throw new Exception(NotFound);
            customer.Orders.Clear();
            return customer;
        }

        [HttpPost]
        public async Task<Customer> CreateCustomer([FromBody] Customer customer)
        {
            try
            {
                JustValidate(customer);

                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();

                return customer;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void JustValidate(Customer customer)
        {
            //throw new NotImplementedException();
        }

        [HttpPut]
        public async Task<Customer> UpdateCustomer([FromBody] Customer customer, int? id)
        {
            try
            {
                JustValidate(customer);

                if (_context.Customers == null)
                    throw new Exception(NotFound);

                Customer? existingCustomer = await JustGetCustomer(id);
                existingCustomer.Name = customer.Name;
                existingCustomer.PhoneNo = customer.PhoneNo;
                //existingCustomer.Orders.Clear();
                customer = existingCustomer;
                _context.Attach(customer).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
                    {
                        throw new Exception(NotFound);
                    }
                    else
                    {
                        throw;
                    }
                }

                return customer;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }

        [HttpDelete]
        public async Task<Customer> DeleteCustomer(int? id)
        {
            try
            {
                if (id == null || _context.Customers == null)
                {
                    throw new Exception(NotFound);
                }
                Customer? customer = await JustGetCustomer(id);
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();

                return customer;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
