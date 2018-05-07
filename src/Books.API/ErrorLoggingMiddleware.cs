using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Books.API
{
    //public class ErrorLoggingMiddleware : IMiddleware
    //{
    //    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    //    {
    //        try
    //        {
    //            await next(context);
    //        }
    //        catch (Exception e)
    //        {
    //            Log.Fatal(e, "An unhandled error happend");
    //        }
    //    }
    //}

    public class ErrorLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"The following error happened: {e.Message}");
                throw;
            }
        }
    }
}
