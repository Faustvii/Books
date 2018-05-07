using Books.EF;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Swagger;
using MediatR;
using Books.CommandHandlers;
using AutoMapper;

namespace Books.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("BooksContext");
            services
                .AddEntityFrameworkNpgsql()
                .AddDbContext<BooksContext>(opts => opts.UseNpgsql(connectionString))
                .AddMvc();

            var commandHandlerAssembly = typeof(AssemblyAnchor).Assembly;

            services.AddAutoMapper(commandHandlerAssembly);
            services.AddMediatR(commandHandlerAssembly);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Books API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseDeveloperExceptionPage();
            //    //app.UseExceptionHandler();
            //}

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Books API V1");
            });


            //app.UseExceptionHandler(errorApp =>
            //{
            //    errorApp.Run(async context =>
            //    {
            //        context.Response.StatusCode = 500; // or another Status accordingly to Exception Type
            //        context.Response.ContentType = "application/json";

            //        var error = context.Features.Get<IExceptionHandlerFeature>();
            //        if (error != null)
            //        {
            //            var ex = error.Error;
            //            Log.Error(ex, "Unhandled exception happend in API");
            //            await context.Response.WriteAsync("Internal server error");
            //        }
            //    });
            //});
            app.UseMvc();
            app.UseMiddleware<ErrorLoggingMiddleware>();

        }
    }
}
