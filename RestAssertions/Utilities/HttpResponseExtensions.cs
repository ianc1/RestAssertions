namespace RestAssertions.Utilities
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Shouldly;

    public static class HttpResponseExtensions
    {
        static HttpResponseExtensions()
        {
            ShouldlyConfiguration.DisableSourceInErrors();
        }

        public static async Task ShouldBe(this HttpResponseMessage httpResponseMessage, HttpStatusCode expectedStatusCode, object expectedContent, string customMessage = null)
        {
            var content = await httpResponseMessage.Content.ReadAsStringAsync();

            httpResponseMessage.StatusCode.ShouldBe(expectedStatusCode, $"Content: {content}");

            var actualJson = JsonUtils.Normalize<Dictionary<string, object>>(content);
            var expectedJson = JsonUtils.Serialize(expectedContent);

            actualJson.ShouldBe(expectedJson, customMessage);
        }

        public static async Task ShouldBe(this HttpResponseMessage httpResponseMessage, HttpStatusCode expectedStatusCode)
        {
            var content = await httpResponseMessage.Content.ReadAsStringAsync();

            httpResponseMessage.StatusCode.ShouldBe(expectedStatusCode, $"Content: {content}");
        }
    }
}
