using Books.EF;

namespace Books.API.IntegrationTests
{
    public static class TestDatabaseManager
    {
        private static bool DatabaseInitializedAlready { get; set; }
        public static void Initialize(BooksContext context)
        {
            if (!DatabaseInitializedAlready)
            {
                context.Database.EnsureCreated();
                //Ensure database is empty
                context.Books.RemoveRange(context.Books);
                context.Authors.RemoveRange(context.Authors);
                context.SaveChanges();
                DatabaseInitializedAlready = true;
            }
        }
    }
}
