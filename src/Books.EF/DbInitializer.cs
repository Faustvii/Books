using System.Linq;

namespace Books.EF
{
    public static class DbInitializer
    {
        public static void Initialize(BooksContext context)
        {
            context.Database.EnsureCreated();
            if (context.Books.Any())
            {
                return; // The db has been seeded;
            }
            var books = new[]
            {
                new Book
                {
                    Isbn = "0345461622",
                    Title = "Pandora's Star (Commonwealth Saga #1)",
                    Summary = "The year is 2380. The Intersolar Commonwealth, a sphere of stars some four hundred light-years in diameter, contains more than six hundred worlds, interconnected by a web of transport \"tunnels\" known as wormholes. At the farthest edge of the Commonwealth, astronomer Dudley Bose observes the impossible: Over one thousand light-years away, a star... vanishes. It does not go supernova. It does not collapse into a black hole. It simply disappears. Since the location is too distant to reach by wormhole, a faster-than-light starship, the Second Chance, is dispatched to learn what has occurred and whether it represents a threat. In command is Wilson Kime, a five-time rejuvenated ex-NASA pilot whose glory days are centuries behind him.",
                    Author = new Author
                    {
                        Name = "Peter F. Hamilton",
                        Description = "Peter F. Hamilton is a British science fiction author. He is best known for writing space opera. As of the publication of his tenth novel in 2004, his works had sold over two million copies worldwide, making him Britain's biggest-selling science fiction author."
                    },
                    Genres = new[]
                    {
                        new Genre
                        {
                            Name = "Science Fiction"
                        },
                        new Genre
                        {
                            Name = "space-opera"
                        },
                        new Genre
                        {
                            Name = "sci-fi"
                        }
                    },
                    Ratings = new[]
                    {
                        new Rating
                        {
                            Score = 4
                        },
                        new Rating
                        {
                            Score = 5
                        },
                        new Rating
                        {
                            Score = 5
                        }
                    }
                }
            };
            context.Books.AddRange(books);
            context.SaveChanges();
        }
    }
}
