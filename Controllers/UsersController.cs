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
using System.Diagnostics;

namespace GOVAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly GOVAPIContext _context;
        public UsersController(GOVAPIContext context) { _context = context; }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers(string searchExpression = null)
        {
            Expression<Func<User, bool>> lambdaExpression = null;
            if (!string.IsNullOrWhiteSpace(searchExpression)) 
            {
                try{ lambdaExpression = DynamicExpressionParser.ParseLambda<User, bool>(new ParsingConfig(), true, searchExpression); }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }
            var queryableUser = this._context.User.AsQueryable();
            if (lambdaExpression != null) { queryableUser = queryableUser.Where(lambdaExpression); }
            
            var users = await queryableUser.ToListAsync();
            return users;
        }

        // GET: api/Users/GetUsersWithRelatedData
        [HttpGet("GetUsersWithRelatedData")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsersWithRelatedData(string searchExpression = null)
        {
            Expression<Func<User, bool>> lambdaExpression = null;
            if (!string.IsNullOrWhiteSpace(searchExpression))
            {
                try { lambdaExpression = DynamicExpressionParser.ParseLambda<User, bool>(new ParsingConfig(), true, searchExpression); }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }
            var queryableUser = this._context.User.AsQueryable();
            if (lambdaExpression != null) { queryableUser = queryableUser.Where(lambdaExpression); }

            var users = await queryableUser.Include(x => x.UserProducts).ThenInclude(x => x.Product).ToListAsync();

            foreach (var user in users)
            {
                foreach (var y in user.UserProducts) { user.ScoreTotal += y.Product.Score; }
            }
            return users;
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null) { return NotFound(); }

            return user;
        }

        // GET: api/Users/GetUserWithRelatedData
        [HttpGet("GetUserWithRelatedData/{id}")]
        public async Task<ActionResult<User>> GetUserWithRelatedData(int id)
        {

            var user = await _context.User.Include(x => x.UserProducts).ThenInclude(x => x.Product).Where(x => x.ID == id).FirstOrDefaultAsync();
            if (user == null) { return NotFound(); }

            foreach (var x in user.UserProducts) { user.ScoreTotal += x.Product.Score; }

            return user;
        }




        // PUT: api/Users/5 // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.ID) { return BadRequest(); }
            _context.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id)) { return NotFound(); }
                else { throw; }
            }
            return NoContent();
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.User.Add(user);
            await _context.SaveChangesAsync();
            _context.Entry(user).State = EntityState.Detached;
            return CreatedAtAction("GetUser", new { id = user.ID }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null) { return NotFound(); }
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        
        private bool UserExists(int id) { return _context.User.Any(e => e.ID == id); }
    }
}
