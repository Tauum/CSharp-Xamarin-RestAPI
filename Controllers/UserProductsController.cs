using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GOVAPI.Data;
using GOVAPI.Models;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;

namespace GOVAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProductsController : ControllerBase
    {
        private readonly GOVAPIContext _context;

        public UserProductsController(GOVAPIContext context) { _context = context; }

        // GET: api/UserProducts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserProduct>>> GetUserProduct(string searchExpression = null)
        {
            Expression<Func<UserProduct, bool>> lambdaExpression = null;
            if (!string.IsNullOrWhiteSpace(searchExpression))
            {
                lambdaExpression = DynamicExpressionParser.ParseLambda<UserProduct, bool>(new ParsingConfig(), true, searchExpression);
            }
            // this was changed VVVVVVVVV
            var queryableUserProducts = this._context.UserProduct.Include((r) => r.User).Include((r) => r.Product).AsQueryable();
            if (lambdaExpression != null) { queryableUserProducts = queryableUserProducts.Where(lambdaExpression); }
            return await queryableUserProducts.ToListAsync();
        }

        // GET: api/UserProducts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserProduct>> GetUserProduct(int id)
        {
            var userProduct = await _context.UserProduct.FindAsync(id);

            if (userProduct == null) { return NotFound(); }

            return userProduct;
        }

        // PUT: api/UserProducts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserProduct(int id, UserProduct userProduct)
        {
            if (id != userProduct.ID) { return BadRequest(); }

            _context.Entry(userProduct).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserProductExists(id)) { return NotFound(); }
                else { throw; }
            }
            return NoContent();
        }

        // POST: api/UserProducts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserProduct>> PostUserProduct(UserProduct userProduct)
        {
            _context.UserProduct.Add(userProduct);
            await _context.SaveChangesAsync(); //drops here in testing

            _context.Entry(userProduct).State = EntityState.Detached;
            return CreatedAtAction("GetUserProduct", new { id = userProduct.ID }, userProduct);
        }

        // DELETE: api/UserProducts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserProduct(int id)
        {
            var userProduct = await _context.UserProduct.FindAsync(id);
            if (userProduct == null) { return NotFound(); }

            _context.UserProduct.Remove(userProduct);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool UserProductExists(int id) { return _context.UserProduct.Any(e => e.ID == id); }
    }
}
