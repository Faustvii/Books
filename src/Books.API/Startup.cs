using AutoMapper;
using Books.API.Filters;
using Books.CommandHandlers;
using Books.EF;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

            services.AddScoped<ValidateModelAttribute>();

            var commandHandlerAssembly = typeof(AssemblyAnchor).Assembly;

            // https://github.com/AutoMapper/AutoMapper.Extensions.Microsoft.DependencyInjection/issues/36
            AutoMapper.ServiceCollectionExtensions.UseStaticRegistration = false;
            services.AddAutoMapper(commandHandlerAssembly);
            services.AddMediatR(commandHandlerAssembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            if (!env.IsEnvironment("Test"))
            {
                app.UseSwagger();

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Books API V1");
                });
            }

            app.UseMvc();
            app.UseMiddleware<ErrorLoggingMiddleware>();
        }
    }
}
