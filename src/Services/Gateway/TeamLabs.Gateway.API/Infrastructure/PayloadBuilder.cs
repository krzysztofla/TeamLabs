using Newtonsoft.Json;

namespace TeamLabs.Gateway.API.Infrastructure
{
    public class PayloadBuilder : IPayloadBuilder
    {
        public async Task<T> ReadAndTransformPayloadAsync<T>(HttpRequest httpRequest) where T : class, new()
        {
            if(httpRequest.Body is null)
            {
                return new T();
            }

            using var streamReader = new StreamReader(httpRequest.Body);
            var payload = await streamReader.ReadToEndAsync();
            return string.IsNullOrWhiteSpace(payload) ? new T() : JsonConvert.DeserializeObject<T>(payload);
        }
    }
}
