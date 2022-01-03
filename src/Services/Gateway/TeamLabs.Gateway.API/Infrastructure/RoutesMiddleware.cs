using System.Runtime.CompilerServices;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using TeamLabs.Gateway.API.Extensions;
using TeamLabs.Gateway.API.Models;

namespace TeamLabs.Gateway.API.Infrastructure
{
    internal sealed class RoutesMiddleware : IMiddleware
    {
        private readonly IDictionary<string, Models.RouteOptions> _routes;
        private readonly IPayloadBuilder _payloadBuilder;
        public RoutesMiddleware(IOptions<RoutesOptions> routes, IPayloadBuilder payloadBuilder)
        {
            _routes = routes.Value.Routes;
            _payloadBuilder = payloadBuilder;
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

            var resourceId = Guid.NewGuid().ToString("N");
            var payload = await _payloadBuilder.ReadAndTransformPayloadAsync<JObject>(context.Request);

            if (context.Request.Method == "POST")
            {
                payload.SetResourceId(resourceId);
            }


        }
    }
}