#pragma warning disable SA1118 // Parameter must not span multiple lines
namespace ExampleWebApi.Tests
{
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    using ExampleWebApi.Tests.Models;

    using RestAssertions;

    using Xunit;

    public class UsersApiTests
    {
        private const string UsersEndpoint = "https://jsonplaceholder.typicode.com/users";
        private const string ValidBearerToken = "FAKE_TOKEN";

        private static readonly HttpClient HttpClient = new HttpClient();

        [Fact]
        public async Task GetUser_should_return_200_ok_and_the_requested_user()
        {
            var response = await HttpClient.TestGet($"{UsersEndpoint}/1", ValidBearerToken);

            response.ShouldBe(HttpStatusCode.OK);
            response.ShouldMatchJson(new
            {
                id = 1,
                username = "Bret",
                name = "Leanne Graham",
                email = "Sincere@april.biz",
                phone = "1-770-736-8031 x56442",
                website = "hildegard.org",
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
                company = new
                {
                    name = "Romaguera-Crona",
                    catchPhrase = "Multi-layered client-server neural-net",
                    bs = "harness real-time e-markets",
                },
            });
        }

        [Fact]
        public async Task GetUser_should_return_200_OK_and_the_id_and_city_properties()
        {
            var response = await HttpClient.TestGet($"{UsersEndpoint}/1", ValidBearerToken);

            response.ShouldBe(HttpStatusCode.OK);
            response.ShouldContainJsonProperty("id", 1);
            response.ShouldContainJsonProperty("address.city", "Gwenborough");
        }

        [Fact]
        public async Task GetUser_should_return_404_not_found_when_the_requested_user_does_not_exist()
        {
            var response = await HttpClient.TestGet($"{UsersEndpoint}/100", ValidBearerToken);

            response.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetUsers_should_return_200_ok_and_a_list_of_users()
        {
            var response = await HttpClient.TestGet($"{UsersEndpoint}?username=Bret&username=Antonette", ValidBearerToken);

            response.ShouldBe(HttpStatusCode.OK);
            response.ShouldMatchJson(new object[]
            {
                new
                {
                    id = 1,
                    username = "Bret",
                    name = "Leanne Graham",
                    email = "Sincere@april.biz",
                    phone = "1-770-736-8031 x56442",
                    website = "hildegard.org",
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
                    company = new
                    {
                        name = "Romaguera-Crona",
                        catchPhrase = "Multi-layered client-server neural-net",
                        bs = "harness real-time e-markets",
                    },
                },
                new
                {
                    id = 2,
                    name = "Ervin Howell",
                    username = "Antonette",
                    email = "Shanna@melissa.tv",
                    address = new
                    {
                        street = "Victor Plains",
                        suite = "Suite 879",
                        city = "Wisokyburgh",
                        zipcode = "90566-7771",
                        geo = new
                        {
                            lat = "-43.9509",
                            lng = "-34.4618",
                        },
                    },
                    phone = "010-692-6593 x09125",
                    website = "anastasia.net",
                    company = new
                    {
                        name = "Deckow-Crist",
                        catchPhrase = "Proactive didactic contingency",
                        bs = "synergize scalable supply-chains",
                    },
                },
            });
        }

        [Fact]
        public async Task GetUsers_should_return_200_ok_and_the_id_and_city_properties()
        {
            var response = await HttpClient.TestGet($"{UsersEndpoint}?username=Bret&username=Antonette", ValidBearerToken);

            response.ShouldBe(HttpStatusCode.OK);
            response.ShouldContainJsonProperty("[0].id", 1);
            response.ShouldContainJsonProperty("[0].company.name", "Romaguera-Crona");
            response.ShouldContainJsonProperty("[1].id", 2);
            response.ShouldContainJsonProperty("[1].company.name", "Deckow-Crist");
        }

        [Fact]
        public async Task GetUsers_should_return_200_ok_and_an_empty_list_when_no_matching_users_are_found()
        {
            var response = await HttpClient.TestGet($"{UsersEndpoint}?username=Scott", ValidBearerToken);

            response.ShouldBe(HttpStatusCode.OK);
            response.ShouldMatchJson(new object[] { });
        }

        [Fact]
        public async Task CreateUser_should_return_201_created_and_the_created_user()
        {
            var user = new User
            {
                Name = "Jon Doe",
                Username = "JonD",
                Email = "jon.doe@company.org",
            };

            var response = await HttpClient.TestPost(UsersEndpoint, user, ValidBearerToken);

            response.ShouldBe(HttpStatusCode.Created);
            user.Id = response.ShouldContainLocationHeaderWithId();
            response.ShouldMatchJson(user);
        }

        [Fact]
        public async Task CreateUser_should_return_201_created_and_contain_the_email_address()
        {
            var user = new User
            {
                Name = "Jon Doe",
                Username = "JonD",
                Email = "jon.doe@company.org",
            };

            var response = await HttpClient.TestPost(UsersEndpoint, user, ValidBearerToken);

            response.ShouldBe(HttpStatusCode.Created);
            response.ShouldContainText("jon.doe@company.org");
        }

        [Fact]
        public async Task UpdateUser_should_return_200_ok_and_the_updated_user()
        {
            var user = new User
            {
                Id = 1,
                Username = "JonD",
                Name = "Jon Doe",
                Email = "jon.doe@company.org",
            };

            var response = await HttpClient.TestPut($"{UsersEndpoint}/1", user, ValidBearerToken);

            response.ShouldBe(HttpStatusCode.OK);
            response.ShouldMatchJson(user);
        }

        [Fact]
        public async Task DeleteUser_should_return_200_ok_when_a_user_is_deleted()
        {
            var response = await HttpClient.TestDelete($"{UsersEndpoint}/1", ValidBearerToken);

            response.ShouldBe(HttpStatusCode.OK); // Ideally the API would return 204 no content.
            response.ShouldMatchJson(new { });
        }
    }
}
