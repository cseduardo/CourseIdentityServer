using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Client.Controllers
{
    public class HomeController : Controller
    {
        //private HttpClient client;
        private readonly IHttpClientFactory clientFactory;
        public HomeController(IHttpClientFactory httpClientFactory)
        {
            //client = httpClientFactory.CreateClient();
            clientFactory = httpClientFactory;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Secret()
        {
            var serverResponse = await AccessTokenRefreshWrapper(() => SecuredGetRequest("https://localhost:44352/secret/index"));

            var apiResponse = await AccessTokenRefreshWrapper(() => SecuredGetRequest("https://localhost:44366/secret/index"));
            //var token = await HttpContext.GetTokenAsync("access_token");
            //var refreshToken = await HttpContext.GetTokenAsync("refresh_token");

            //var serverClient = clientFactory.CreateClient();

            //serverClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            //var serverResponse = await serverClient.GetAsync("https://localhost:44352/secret/index");

            //await RefreshAccessToken();

            //var apiClient = clientFactory.CreateClient();

            //token = await HttpContext.GetTokenAsync("access_token");
            //refreshToken = await HttpContext.GetTokenAsync("refresh_token");

            //apiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            //var apiResponse = await apiClient.GetAsync("https://localhost:44366/secret/index");

            return View();
        }

        //public async Task<string> RefreshAccessToken()
        //{
        //    var refreshToken = await HttpContext.GetTokenAsync("refresh_token");

        //    var refreshTokenClient = clientFactory.CreateClient();
        //    var requestData = new Dictionary<string, string>
        //    {
        //        ["grant_type"] = "refresh_token",
        //        ["refresh_token"] = refreshToken
        //    };
        //    var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44352/oauth/token")
        //    {
        //        Content = new FormUrlEncodedContent(requestData)
        //    };

        //    var basicCredentials = "username:password";
        //    var encodeCredentials = Encoding.UTF8.GetBytes(basicCredentials);
        //    var base64Credentials = Convert.ToBase64String(encodeCredentials);

        //    request.Headers.Add("Authorization", $"Basic {base64Credentials}");

        //    var response = await refreshTokenClient.SendAsync(request);
        //    var responseString = await response.Content.ReadAsStringAsync();
        //    var responseData = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseString);

        //    var newAccessToken = responseData.GetValueOrDefault("access_token");
        //    var newRefreshToken = responseData.GetValueOrDefault("refresh_token");

        //    var authInfo = await HttpContext.AuthenticateAsync("ClientCookie");

        //    authInfo.Properties.UpdateTokenValue("access_token", newAccessToken);
        //    authInfo.Properties.UpdateTokenValue("refresh_token", newRefreshToken);

        //    await HttpContext.SignInAsync("ClientCookie", authInfo.Principal, authInfo.Properties);

        //    return "";
        //}


        private async Task<HttpResponseMessage> SecuredGetRequest(string uri)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var client = clientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            return await client.GetAsync(uri);
        }

        public async Task<HttpResponseMessage> AccessTokenRefreshWrapper(Func<Task<HttpResponseMessage>> intialRequest)
        {
            var response = await intialRequest();

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await RefreshAccessToken();
                response = await intialRequest();
            }
            return response;
        }

        private async Task RefreshAccessToken()
        {
            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");

            var refreshTokenClient = clientFactory.CreateClient();
            var requestData = new Dictionary<string, string>
            {
                ["grant_type"] = "refresh_token",
                ["refresh_token"] = refreshToken
            };
            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44352/oauth/token")
            {
                Content = new FormUrlEncodedContent(requestData)
            };

            var basicCredentials = "username:password";
            var encodeCredentials = Encoding.UTF8.GetBytes(basicCredentials);
            var base64Credentials = Convert.ToBase64String(encodeCredentials);

            request.Headers.Add("Authorization", $"Basic {base64Credentials}");

            var response = await refreshTokenClient.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseString);

            var newAccessToken = responseData.GetValueOrDefault("access_token");
            var newRefreshToken = responseData.GetValueOrDefault("refresh_token");

            var authInfo = await HttpContext.AuthenticateAsync("ClientCookie");

            authInfo.Properties.UpdateTokenValue("access_token", newAccessToken);
            authInfo.Properties.UpdateTokenValue("refresh_token", newRefreshToken);

            await HttpContext.SignInAsync("ClientCookie", authInfo.Principal, authInfo.Properties);
        }
    }
}
