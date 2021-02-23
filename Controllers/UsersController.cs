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

        public UsersController(GOVAPIContext context)
        {
            _context = context;
        }

        //// POST: api/Login
        //[Route("LogIn")]
        //[HttpPost]
        //public async Task<ActionResult<Guid>> LogIn(string email, string password) // this generates a GUID if user login is correct
        //{
        //    User user = this._context.User.FirstOrDefault<User>(user => user.Email.ToLower().Equals(email));
        //    if (user is null) { return Unauthorized(Guid.Empty); }
        //    if (Security.VerifyHash(password, user.Password)) { return Ok(Guid.NewGuid());}
        //    return Unauthorized(Guid.Empty);
        //}

        // GET: api/Users
          [Route("GetUsers")]
          [HttpGet]
          public async Task<ActionResult<IEnumerable<User>>> GetUsers(string searchExpression = null) //this doesnt work
          {
              Expression<Func<User, bool>> lambdaExpression = null;
              if (!string.IsNullOrWhiteSpace(searchExpression))
              {
                  lambdaExpression = DynamicExpressionParser.ParseLambda<User, bool>(new ParsingConfig(), true, searchExpression);
              }
              var queryableUser = this._context.User.AsQueryable();
              if (lambdaExpression != null)
              {
                  queryableUser = queryableUser.Where(lambdaExpression);
              }
              var list = await queryableUser.ToListAsync();
              return list;
          } 
        


        //public class Rev {
        //    public string Username {get;set;}
        //    public int ProductID {get;set;}
        //    public string Description {get;set;}
        //}

        //[Route("GetReviews")]
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Rev>>> GetRevs() {
        //    return Ok(this._context.User
        //        .Join(
        //            this._context.Review,
        //            user2 => user2.ID,
        //            review => review.UserID,
        //            (user2, review) => new Rev{
        //                Username = user2.Username,
        //                ProductID = review.ProductID,
        //                Description = review.Description
        //            }
        //        ).ToList());
        //}

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("PutUser{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.ID)
            {
                return BadRequest();
            }

            _context.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Route("PostUser")]
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.ID }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.ID == id);
        }
    }
}
