using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GOVAPI.Data;
using GOVAPI.Models;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace GOVAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly GOVAPIContext _context;

        public ProductsController(GOVAPIContext context) { _context = context; }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct(string searchExpression = null)
        {
            Expression<Func<Product, bool>> lambdaExpression = null;

            if (!string.IsNullOrWhiteSpace(searchExpression))
            {
                try
                {
                    lambdaExpression = DynamicExpressionParser.ParseLambda<Product, bool>(new ParsingConfig(), true, searchExpression);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }

            var queryableProducts = this._context.Product.AsQueryable();

            if (lambdaExpression != null) { queryableProducts = queryableProducts.Where(lambdaExpression); }

            var list = await queryableProducts.ToListAsync();

            return list;
        }
        // GET: api/Products/GetProductsWithRelatedData
        [HttpGet("GetProductsWithRelatedData")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsWithRelatedData(string searchExpression = null)
        {
            Expression<Func<Product, bool>> lambdaExpression = null;

            if (!string.IsNullOrWhiteSpace(searchExpression))
            {
                lambdaExpression = DynamicExpressionParser.ParseLambda<Product, bool>(new ParsingConfig(), true, searchExpression);
            }

            var queryableProducts = this._context.Product.Include(x => x.Image).Include(x => x.Category).AsNoTracking().AsQueryable();

            if (lambdaExpression != null) { queryableProducts = queryableProducts.Where(lambdaExpression); }

            var list = await queryableProducts.ToListAsync();

            return list;
        }
        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Product.FindAsync(id);

            if (product == null) { return NotFound(); }

            return product;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.ID) { return BadRequest(); }

            // get the product as it currently exists in the database, 
            // we'll be updating this with the one posted to the method

            _context.Entry(product).State = EntityState.Modified;

            var existingProduct = await _context.Product.Include(x => x.Category).Where(x => x.ID == id).SingleOrDefaultAsync();

            try { await _context.SaveChangesAsync(); }

                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(id)) { return NotFound(); }

                    else { throw; }
                }
                return NoContent();
        }


        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            // enumerate the actors for this new Product and attach them
            // this ensures EF associates existing Actor records with this Product
            // instead of erroneously trying to create new Actor records

            if (product.Category != null) {  _context.Category.Attach(product.Category); }

            _context.Product.Add(product);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.ID }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteProduct(int id)
            {
                var product = await _context.Product.FindAsync(id);
                if (product == null) { return NotFound(); }

                _context.Product.Remove(product);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            private bool ProductExists(int id) { return _context.Product.Any(e => e.ID == id); }
        }
    }
