using Books.EF;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using System;

namespace Books.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .Enrich.WithDemystifiedStackTraces()
                .WriteTo.Elasticsearch(new Serilog.Sinks.Elasticsearch.ElasticsearchSinkOptions(new Uri("https://cloud.humio.com:443/api/v1/dataspaces/sandbox/ingest/elasticsearch"))
                {
                    ModifyConnectionSettings = c => c.BasicAuthentication("jpDTaa5C3JBBVTJehgvGde6HStMZsvIQrIgUI9SmHZOV", ""),
                })
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                Log.Information("Starting web host");
                var host = BuildWebHost(args);
                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    try
                    {
                        var context = services.GetRequiredService<BooksContext>();
                        DbInitializer.Initialize(context);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "An error occurred trying to initialize database");
                    }
                }

                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }

        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseSerilog()
                .UseKestrel()
                .UseStartup<Startup>()
                .Build();
    }
}
