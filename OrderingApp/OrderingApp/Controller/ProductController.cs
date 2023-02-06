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
    public class ProductController
    {
        private readonly string NotFound = "Not Found";
        private readonly OrderingAppDataContext _context;

        public ProductController(OrderingAppDataContext context) => _context = context;

        [HttpGet]
        public async Task<IList<Product>> GetAllProductList()
        {
            try
            {
                var products = await _context.Products.ToListAsync();
                products.ForEach(x => x.OrderItems.Clear());
                return products;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<Product> GetProduct(int? id)
        {
            try
            {
                if (id == null || _context.Products == null)
                    throw new Exception(NotFound);
                Product? product = await JustGetProduct(id);
                product.OrderItems.Clear();
                return product;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<Product> JustGetProduct(int? id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
                throw new Exception(NotFound);
            product.OrderItems.Clear();
            return product;
        }

        [HttpPost]
        public async Task<Product> CreateProduct([FromBody] Product product)
        {
            try
            {
                JustValidate(product);

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                return product;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void JustValidate(Product product)
        {
            //throw new NotImplementedException();
        }

        [HttpPut]
        public async Task<Product> UpdateProduct([FromBody] Product product, int? id)
        {
            try
            {
                JustValidate(product);

                if (_context.Products == null)
                    throw new Exception(NotFound);

                Product? existingProduct = await JustGetProduct(id);
                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                //product.OrderItems.Clear();
                product = existingProduct;
                _context.Attach(product).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        throw new Exception(NotFound);
                    }
                    else
                    {
                        throw;
                    }
                }

                return product;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        [HttpDelete]
        public async Task<Product> DeleteProduct(int? id)
        {
            try
            {
                if (id == null || _context.Products == null)
                {
                    throw new Exception(NotFound);
                }
                Product? product = await JustGetProduct(id);
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                return product;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
