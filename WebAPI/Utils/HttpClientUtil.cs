using System.Text;
using System.Text.Json;
using System.Web;

namespace WebAPI.Utils
{
    public class HttpClientUtil(HttpClient httpClient)
    {
        public async Task<TResponse> GetRequestAsync<TResponse, TRequest>(string requestUri, object parameters = null)
        {
            try
            {
                if (parameters != null)
                {
                    var queryString = ToQueryString(parameters);
                    requestUri = $"{requestUri}?{queryString}&time={DateTime.Now.Ticks}&sig=PhapDienCloud";
                }

                HttpResponseMessage response = await httpClient.GetAsync(requestUri);

                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrWhiteSpace(responseContent))
                    {
                        return JsonSerializer.Deserialize<TResponse>(responseContent, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                    }
                }
            }
            catch
            {
            }

            return default;
        }

        public async Task<TResponse> PostRequestAsync<TResponse, TRequest>(string requestUri, TRequest content)
        {
            try
            {
                var jsonContent = new StringContent(JsonSerializer.Serialize(content).Replace("\"[", "[").Replace("]\"", "]"), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(requestUri, jsonContent);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<TResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch
            {
            }

            return default;
        }

        private string ToQueryString(object parameters)
        {
            var properties = from p in parameters.GetType().GetProperties()
                             where p.GetValue(parameters, null) != null
                             select $"{HttpUtility.UrlEncode(p.Name)}={HttpUtility.UrlEncode(p.GetValue(parameters, null).ToString())}";

            return string.Join("&", properties);
        }
    }
}
