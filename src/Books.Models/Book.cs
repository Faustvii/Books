using System.Collections.Generic;

namespace Books
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Isbn { get; set; }
        public string Summary { get; set; }

        public virtual Author Author { get; set; }
        public int AuthorId { get; set; }

        public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
        public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();
    }
}