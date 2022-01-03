namespace TeamLabs.Gateway.API.Models
{
    record class RoutesOptions
    {
        public IDictionary<string, RouteOptions> Routes {get; set;}
    }
}