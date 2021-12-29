using System.Runtime.CompilerServices;
using Microsoft.Extensions.Options;
using TeamLabs.Gateway.API.Models;

namespace TeamLabs.Gateway.API.Infrastructure
{
    internal sealed class RoutesMiddleware : IMiddleware
    {
        private readonly IDictionary<string, Models.RouteOptions> _routes;
        public RoutesMiddleware(IOptions<RoutesOptions> routes)
        {
            _routes = routes.Value.Routes;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if(_routes is null || !_routes.Any()){
                await next(context);
                return;
            }

            var key = $"{context.Request.Method} {context.Request.Path}";
            if(!_routes.TryGetValue(key, out var route)) {
                await next(context);
                return;
            }


        }
    }
}