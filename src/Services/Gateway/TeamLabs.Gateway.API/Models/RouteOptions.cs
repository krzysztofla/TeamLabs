using System.Globalization;
namespace TeamLabs.Gateway.API.Models
{
    internal sealed class RouteOptions
    {
        public string? Exchange {get; set; }

        public string? RoutingKey { get; set; }
    }
}