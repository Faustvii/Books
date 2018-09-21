using Books.EF;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Books.API.IntegrationTests
{
    public class AddBooksTests : TestBase
    {
        [Theory]
        [MemberData(nameof(TestData), parameters: 500)]
        public async Task GetBookByTitle(string title)
        {
            // Arrange
            var t = TestEnvironment.ServiceProvider.GetService<BooksContext>();
            await Context.AddAsync(new Book
            {
                Title = title,
                Author = new Author
                {
                    Name = "Test Author",
                    Description = "Makes tests awesome"
                }
            });
            await Context.SaveChangesAsync();

            // Act
            var result = Context.Books.FirstOrDefault(a => a.Title == title);

            // Assert
            result.Should().NotBeNull();
            result.Title.Should().Be(title);
            Context.Books.Count().Should().Be(1);
        }

        public static IEnumerable<object[]> TestData(int numTests)
        {
            var allData = new List<object[]>();
            for (var i = 0; i < numTests; i++)
            {
                allData.Add(new object[] { $"Book {i}" });
            }
            return allData;
        }
    }
}
