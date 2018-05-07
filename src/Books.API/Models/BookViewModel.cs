using System.Collections.Generic;

namespace Books.API.Models
{
    public class BookViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Isbn { get; set; }
        public string AuthorName { get; set; }
        public int AuthorId { get; set; }
        public double Rating { get; set; }
        public ICollection<Genre> Genres { get; set; }
    }
}
