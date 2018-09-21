using System.Threading.Tasks;
using AutoMapper;
using Books.CommandHandlers.Commands;
using Books.EF;
using MediatR;

namespace Books.CommandHandlers.Handlers
{
    public class CreateBookHandler : AsyncRequestHandler<CreateBook, int>
    {
        private readonly BooksContext _context;
        private readonly IMapper _mapper;

        public CreateBookHandler(BooksContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        protected override async Task<int> HandleCore(CreateBook request)
        {
            var book = _mapper.Map<Book>(request);
            await _context.AddAsync(book);
            await _context.SaveChangesAsync();
            return book.Id;
        }
    }
}
