# RestAssertions
A collection of HTTP response assertions to simplify the testing of REST endpoints.

Four `HttpClient` extension methods are provided to test your endpoints:
* `TestGet(string url, string bearerToken)`
* `TestPost(string url, object model, string bearerToken)`
* `TestPut(string url, object model, string bearerToken)`
* `TestDelete(string url, string bearerToken)`

These extension methods return an instance of `HttpResponseAssertions` which provides the following assertions:
* `ShouldBe(HttpStatusCode expectedStatusCode)`
* `ShouldMatchJson(object expectedContent)`
* `ShouldHaveHeader(String expectedName, String expectedValue)`
* `int ShouldContainLocationHeaderWithId()`
* `Guid ShouldContainLocationHeaderWithGuid()`


## Get Started

```c#
var response = await httpClient.TestGet("http://localhost/users/1", bearerToken);

response.ShouldBe(HttpStatusCode.OK);
response.ShouldMatchJson(new User
{
    Id = 1,
    Username = "Leanne",
    Name = "Leanne Graham",
    EMail = "Leanne.Graham@april.biz",
});
```

Example test failure message
```
Message: RestAssertions.RestAssertionException : 

Expected HTTP response content to be:
     1:  {
     2:    "email": "Leanne.Graham@april.biz",
     3:    "id": 1,
     4:    "name": "Leanne Graham",
*    5:    "username": "Leanne"
     6:  }

but was:
     1:  {
     2:    "email": "Leanne.Graham@april.biz",
     3:    "id": 1,
     4:    "name": "Leanne Graham",
*    5:    "username": "Bret"
     6:  }

Additional information

Difference:
         Line 5

Status:
         200 OK

Headers:
     1:  Accept-Ranges: bytes
     2:  Access-Control-Allow-Credentials: true
     3:  Cache-Control: public, max-age=14400
     4:  CF-Cache-Status: HIT
     5:  CF-RAY: 4c05e5ac5bcfce19-LHR
     6:  Connection: keep-alive
     7:  Content-Length: 509
     8:  Content-Type: application/json; charset=utf-8
     9:  Date: Sun, 31 Mar 2019 22:53:02 GMT
    10:  ETag: W/"1fd-+2Y3G3w049iSZtw5t1mzSnunngE"
    11:  Expect-CT: max-age=604800, report-uri="https://report-uri.cloudflare.com/cdn-cgi/beacon/expect-ct"
    12:  Expires: Mon, 01 Apr 2019 02:53:02 GMT
    13:  Pragma: no-cache
    14:  Server: cloudflare
    15:  Vary: Origin, Accept-Encoding
    16:  Via: 1.1 vegur
    17:  X-Content-Type-Options: nosniff
    18:  X-Powered-By: Express

Content:
     1:  {
     2:    "email": "Leanne.Graham@april.biz",
     3:    "id": 1,
     4:    "name": "Leanne Graham",
     5:    "username": "Bret"
     6:  }
```
