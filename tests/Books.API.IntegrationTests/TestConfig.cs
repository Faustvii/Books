using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Books.API.IntegrationTests
{
    /// <summary>
    /// Sets up configuration that should _ONLY_ be used when running TESTS.
    /// </summary>
    public static class TestConfig
    {
        public static void ConfigureServices(WebHostBuilderContext hostingContext, IServiceCollection services)
        {
            //No test specific things yet.
        }
    }
}
