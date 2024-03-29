﻿using System;
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
                try { lambdaExpression = DynamicExpressionParser.ParseLambda<Product, bool>(new ParsingConfig(), true, searchExpression); }
                catch(Exception e) { Console.WriteLine(e.Message); }
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

        // GET: api/Products/GetProductsForUser
        [HttpGet("GetProductsForUser/{id}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsForUser(int id)
        {
            var list = await _context.Review.Where(x => x.UserID == id).Select(y => y.Product).Distinct().ToListAsync();
            if (list == null) { return NotFound(); }
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
            _context.Entry(product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
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
            if (product.Category != null) {  _context.Category.Attach(product.Category); }
            _context.Product.Add(product);
            await _context.SaveChangesAsync();

            _context.Entry(product).State = EntityState.Detached;
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
