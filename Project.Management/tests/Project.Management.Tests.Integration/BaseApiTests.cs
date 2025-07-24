using System.Text;
using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace Project.Management.Tests.Integration
{
    public class BaseApiTests(TestApiFixture fixture) : IClassFixture<TestApiFixture>
    {
        private readonly HttpClient _client = fixture.Client;

        private static async Task<T> GetResponseMessage<T>(HttpResponseMessage httpResponse)
        {
            var responseContent = await httpResponse.Content.ReadAsStringAsync();
            var jObject = JObject.Parse(responseContent);

            return (jObject.ContainsKey("data") ? jObject["data"].ToObject<T>() : jObject["message"].ToObject<T>());
        }

        public async Task<T> PutAsync<T>(string url, object value)
        {
            var json = JsonSerializer.Serialize(value);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PutAsync(url, content);

            return await GetResponseMessage<T>(response);
        }

        public async Task<T> PostAsync<T>(string url, object value)
        {
            var json = JsonSerializer.Serialize(value);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(url, content);

            return await GetResponseMessage<T>(response);
        }

        public async Task<T> GetAsync<T>(string url)
        {
            var response = await _client.GetAsync(url);

            return await GetResponseMessage<T>(response);
        }

        public async Task<T> DeleteAsync<T>(string url)
        {
            var response = await _client.DeleteAsync(url);

            return await GetResponseMessage<T>(response);
        }
    }
}
