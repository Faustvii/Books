﻿using Books.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Authors")]
    public class AuthorsController : Controller
    {
        private readonly BooksContext _context;

        public AuthorsController(BooksContext context)
        {
            _context = context;
        }

        // GET: api/Authors
        [HttpGet]
        public IEnumerable<Author> GetAuthors()
        {
            return _context.Authors;
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthor([FromRoute] int id)
        {
            var author = await GetAuthorById(id).ConfigureAwait(false);
            if (author == null)
            {
                return NotFound();
            }

            return Ok(author);
        }

        private async Task<Author> GetAuthorById(int id)
        {
            var author = await _context.Authors.SingleOrDefaultAsync(m => m.Id == id);

            if (author == null)
            {
                return null;
            }
            return author;
        }

        // PUT: api/Authors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor([FromRoute] int id, [FromBody] Author author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != author.Id)
            {
                return BadRequest();
            }

            _context.Entry(author).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorExists(id))
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

        // POST: api/Authors
        [HttpPost]
        public async Task<IActionResult> PostAuthor([FromBody] Author author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAuthor", new { id = author.Id }, author);
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor([FromRoute] int id)
        {
            var author = await GetAuthorById(id).ConfigureAwait(false);
            if (author == null)
            {
                return NotFound();
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return Ok(author);
        }

        private bool AuthorExists(int id)
        {
            return _context.Authors.Any(e => e.Id == id);
        }
    }
}