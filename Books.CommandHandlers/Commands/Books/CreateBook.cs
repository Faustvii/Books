using MediatR;

namespace Books.CommandHandlers.Commands
{
    public class CreateBook : IRequest<int>
    {
        public string Title { get; set; }
        public string Isbn { get; set; }
        public string Summary { get; set; }
        public int AuthorId { get; set; }
    }
}
