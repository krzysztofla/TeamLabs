using System.Globalization;
namespace TeamLabs.Gateway.API.Models
{
    record RouteOptions
    {
        public string? Exchange {get; set; }

        public string? RoutingKey { get; set; }
    }
}