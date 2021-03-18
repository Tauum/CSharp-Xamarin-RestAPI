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
    public class ImagesController : ControllerBase
    {
        private readonly GOVAPIContext _context;

        public ImagesController(GOVAPIContext context) {  _context = context; }

        // GET: api/Images
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Image>>> GetImage(string searchExpression = null)
        {
            Expression<Func<Image, bool>> lambdaExpression = null;
            if (!string.IsNullOrWhiteSpace(searchExpression))
            {
                lambdaExpression = DynamicExpressionParser.ParseLambda<Image, bool>(new ParsingConfig(), true, searchExpression);
            }
            var queryableImages = this._context.Image.AsQueryable();
            if (lambdaExpression != null) { queryableImages = queryableImages.Where(lambdaExpression); }
            return await queryableImages.ToListAsync();
        }


        // GET: api/Images/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Image>> GetImage(int id)
        {
            var Image = await _context.Image.FindAsync(id);

            if (Image == null) { return NotFound(); }

            return Image;
        }

        // PUT: api/Images/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImage(int id, Image Image)
        {
            if (id != Image.ID) { return BadRequest(); }

            _context.Entry(Image).State = EntityState.Modified;

            try { await _context.SaveChangesAsync();}
            catch (DbUpdateConcurrencyException)
            {
                if (!ImageExists(id)) { return NotFound();}
                else { throw; }
            }

            return NoContent();
        }

        // POST: api/Images
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Image>> PostImage(Image Image)
        {
            _context.Image.Add(Image);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetImage", new { id = Image.ID }, Image);
        }

        // DELETE: api/Images/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var Image = await _context.Image.FindAsync(id);
            if (Image == null) { return NotFound(); }

            _context.Image.Remove(Image);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ImageExists(int id) { return _context.Image.Any(e => e.ID == id); }
    }
}
