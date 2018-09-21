using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.IO;
using System.Net.Http;
using System.Reflection;

namespace Books.API.IntegrationTests
{
    public sealed class TestEnvironment<TStartup> where TStartup : class
    {
        private readonly string ContentRootPath;

        public TestServer Server { get; }

        public HttpClient Client { get; }

        public IServiceProvider ServiceProvider => Server?.Host?.Services;

        /// <summary>
        /// Creates a new instance of the TestEnvironment class.
        /// </summary>
        /// <param name="targetProjectRelativePath">
        /// Defines the relative path of the target project to test.
        /// e.g. src, samples, test, or test/Websites
        /// Can be useful if the automatic detection does not work.
        /// </param>
        public TestEnvironment(string targetProjectRelativePath = null)
        {
            ContentRootPath = GetProjectPath(typeof(TStartup).GetTypeInfo().Assembly, targetProjectRelativePath);
            if (ContentRootPath == null)
            {
                throw new InvalidOperationException("Target project can not be located. Try specify the content root path explicitly.");
            }

            Server = CreateTestServer();
            Client = Server.CreateClient();
        }

        private TestServer CreateTestServer()
        {
            return new TestServer
            (
                new WebHostBuilder()
                    .UseContentRoot(ContentRootPath)
                    .UseEnvironment("Test")
                    .ConfigureAppConfiguration(Config.ConfigureAppConfiguration)
                    .ConfigureServices(TestConfig.ConfigureServices)
                    .UseStartup<TStartup>()
            );
        }

        /// <summary>
        /// Gets the full path to the target project path that we wish to test
        /// </summary>
        /// <param name="startupAssembly">The target project's assembly.</param>
        /// <param name="targetRelativePath">
        /// The parent directory of the target project.
        /// e.g. src, samples, test, or test/Websites
        /// </param>
        /// <returns>The full path to the target project.</returns>
        private static string GetProjectPath(Assembly startupAssembly, string targetRelativePath = null)
        {
            // Get name of the target project which we want to test
            var projectName = startupAssembly.GetName().Name;

            // Get currently executing test project path
            var applicationBasePath = PlatformServices.Default.Application.ApplicationBasePath;

            if (targetRelativePath != null)
            {
                targetRelativePath = Path.Combine(targetRelativePath, projectName);
            }

            // Find the folder which contains the solution file. We then use this information to find the target
            // project which we want to test.
            var targetDirectoryInfo = new DirectoryInfo(applicationBasePath);
            do
            {
                var testDirectoryInfo =
                    new DirectoryInfo(Path.Combine(targetDirectoryInfo.FullName, targetRelativePath ?? projectName));

                if (testDirectoryInfo.Exists)
                {
                    return Path.GetFullPath(testDirectoryInfo.FullName);
                }

                targetDirectoryInfo = targetDirectoryInfo.Parent;
            }
            while (targetDirectoryInfo?.Parent != null);

            return null;
        }
    }
}