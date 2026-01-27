using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Web;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace Project.Management.Tests.Integration
{
    public class BaseApiTests: IClassFixture<TestApiFixture>
    {
        private readonly HttpClient _client;

        public BaseApiTests(TestApiFixture fixture)
        {
            _client ??= fixture.Client;
        }

        private static async Task<T> GetResponseMessage<T>(HttpResponseMessage httpResponse)
        {
            var responseContent = await httpResponse.Content.ReadAsStringAsync();
            var jObject = JObject.Parse(responseContent);

            return (jObject.ContainsKey("data") ? jObject["data"].ToObject<T>() : jObject["message"].ToObject<T>());
        }

        public async Task<T> PutAsync<T>(string url, object value)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(value);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PutAsync(url, content);

            return await GetResponseMessage<T>(response);
        }

        public async Task<T> PostAsync<T>(string url, object value)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(value);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(url, content);

            return await GetResponseMessage<T>(response);
        }

        public async Task<T> GetAsync<T>(string url, object request = null)
        {
            if (request != null)
            {
                List<string> jsonList = [];
                var settings = new JsonSerializerSettings();

                var jsonObject = JObject.Parse(JsonConvert.SerializeObject(request, settings));

                foreach (var item in jsonObject)
                {
                    if (item.Value.HasValues)
                    {
                        foreach (var childItem in item.Value.ToArray())
                            jsonList.Add($"{HttpUtility.UrlEncode(item.Key)}={HttpUtility.UrlEncode(childItem.ToString())}");
                    }
                    else
                        jsonList.Add($"{HttpUtility.UrlEncode(item.Key)}={HttpUtility.UrlEncode(item.Value.ToString())}");
                }

                url = $"{url}?{string.Join('&', jsonList)}";
            }

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
