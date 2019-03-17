#pragma warning disable SA1118 // Parameter must not span multiple lines
namespace ExampleWebApi.Tests
{
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using RestAssertions.Utilities;
    using Xunit;

    public class UsersApiTest
    {
        private const string UsersEndpoint = "https://jsonplaceholder.typicode.com/users";
        private const string ValidBearerToken = "FAKE_TOKEN";

        private static readonly HttpClient HttpClient = new HttpClient();

        [Fact]
        public async Task GetUser_should_return_200_ok_and_the_requested_user()
        {
            var response = await HttpClient.GetAsync($"{UsersEndpoint}/1", ValidBearerToken);

            await response.ShouldBe(HttpStatusCode.OK, new
            {
                id = 1,
                name = "Leanne Graham",
                username = "Bret",
                email = "Sincere@april.biz",
                address = new
                {
                    street = "Kulas Light",
                    suite = "Apt. 556",
                    city = "Gwenborough",
                    zipcode = "92998-3874",
                    geo = new
                    {
                        lat = "-37.3159",
                        lng = "81.1496",
                    },
                },
                phone = "1-770-736-8031 x56442",
                website = "hildegard.org",
                company = new
                {
                    name = "Romaguera-Crona",
                    catchPhrase = "Multi-layered client-server neural-net",
                    bs = "harness real-time e-markets",
                }
            });
        }
    }
}
