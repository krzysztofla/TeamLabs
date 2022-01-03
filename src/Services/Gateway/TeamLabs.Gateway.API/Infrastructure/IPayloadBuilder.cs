 namespace TeamLabs.Gateway.API.Infrastructure
{
    public interface IPayloadBuilder
    {
        public Task<T> ReadAndTransformPayloadAsync<T>(HttpRequest httpRequest) where T : class, new();
    }
}
