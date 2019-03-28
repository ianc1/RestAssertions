﻿namespace RestAssertions.Utilities
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Net.Mime;
    using System.Text;
    using System.Threading.Tasks;

    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseAssertions> GetAsync(this HttpClient httpClient, string uri, string token)
        {
            var response = await httpClient.SendAsync(CreateRequest(httpClient, HttpMethod.Get, uri, token));

            return await HttpResponseAssertions.Create(response);
        }

        public static async Task<HttpResponseAssertions> PostAsJsonAsync<T>(this HttpClient httpClient, string uri, T model, string token)
        {
            var request = CreateRequest(httpClient, HttpMethod.Post, uri, token);

            AddJsonContent(request, model);

            var response = await httpClient.SendAsync(request);

            return await HttpResponseAssertions.Create(response);
        }

        public static async Task<HttpResponseAssertions> PutAsJsonAsync<T>(this HttpClient httpClient, string uri, T model, string token)
        {
            var request = CreateRequest(httpClient, HttpMethod.Put, uri, token);

            AddJsonContent(request, model);

            var response = await httpClient.SendAsync(request);

            return await HttpResponseAssertions.Create(response);
        }

        public static async Task<HttpResponseAssertions> DeleteAsync(this HttpClient httpClient, string uri, string token)
        {
            var response = await httpClient.SendAsync(CreateRequest(httpClient, HttpMethod.Delete, uri, token));

            return await HttpResponseAssertions.Create(response);
        }

        private static HttpRequestMessage CreateRequest(HttpClient httpClient, HttpMethod method, string uri, string token)
        {
            if (!Uri.TryCreate(uri, UriKind.Absolute, out var requestUri))
            {
                requestUri = new Uri(httpClient.BaseAddress, uri);
            }

            var request = new HttpRequestMessage(method, requestUri);

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return request;
        }

        private static void AddJsonContent<T>(HttpRequestMessage request, T model)
        {
            request.Content = new StringContent(JsonUtils.Serialize(model), Encoding.UTF8, MediaTypeNames.Application.Json);
        }
    }
}
