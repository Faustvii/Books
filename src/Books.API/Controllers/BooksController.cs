using Books.API.Models;
using Books.CommandHandlers.Commands;
using Books.EF;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.API.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class BooksController : Controller
    {
        private readonly BooksContext _context;
        private readonly IMediator _mediator;

        public BooksController(BooksContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<BookViewModel> Get()
        {
            return _context.Set<Book>().Select(x => new BookViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Summary = x.Summary,
                Isbn = x.Isbn,
                AuthorName = x.Author.Name ?? string.Empty,
                AuthorId = x.AuthorId,
                Genres = x.Genres.ToList(),
                Rating = x.Ratings.Any() ? x.Ratings.Average(t => t.Score) : 0
            });
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Book GetBook([FromRoute] int id)
        {
            return _context.Set<Book>().Include(x => x.Genres).Include(x => x.Ratings).FirstOrDefault(t => t.Id == id);
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateBook model)
        {
            var id = await _mediator.Send(model);
            return CreatedAtAction("GetBook", new { id }, model);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
            throw new NotSupportedException();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _context.Remove(new Book { Id = id });
            _context.SaveChanges();
        }
    }
}
