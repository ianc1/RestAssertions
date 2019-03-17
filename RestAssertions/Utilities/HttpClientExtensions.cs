namespace RestAssertions.Utilities
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    public static class HttpClientExtensions
    {
        public static Task<HttpResponseMessage> GetAsync(this HttpClient httpClient, string uri, string token)
        {
            return httpClient.SendAsync(CreateRequest(httpClient, HttpMethod.Get, uri, token));
        }

        public static Task<HttpResponseMessage> PostAsJsonAsync<T>(this HttpClient httpClient, string uri, T model, string token)
        {
            var request = CreateRequest(httpClient, HttpMethod.Post, uri, token);

            AddJsonContent(request, model);

            return httpClient.SendAsync(request);
        }

        public static Task<HttpResponseMessage> PutAsJsonAsync<T>(this HttpClient httpClient, string uri, T model, string token)
        {
            var request = CreateRequest(httpClient, HttpMethod.Put, uri, token);

            AddJsonContent(request, model);

            return httpClient.SendAsync(request);
        }

        public static Task<HttpResponseMessage> DeleteAsync(this HttpClient httpClient, string uri, string token)
        {
            return httpClient.SendAsync(CreateRequest(httpClient, HttpMethod.Delete, uri, token));
        }

        private static HttpRequestMessage CreateRequest(HttpClient httpClient, HttpMethod method, string uri, string token)
        {
            var requestUri = new Uri(uri);

            if (!requestUri.IsAbsoluteUri)
            {
                requestUri = new Uri(httpClient.BaseAddress, uri);
            }

            var request = new HttpRequestMessage(method, requestUri);

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return request;
        }

        private static void AddJsonContent<T>(HttpRequestMessage request, T model)
        {
            request.Content = new StringContent(JsonUtils.Serialize(model), Encoding.UTF8, "application/json");
        }
    }
}
