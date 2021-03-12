using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiTwo.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public HomeController(IHttpClientFactory httpFactory)
        {
            httpClientFactory = httpFactory;
        }

        [Route("/")]
        public async Task<IActionResult> Index()
        {
            //retrive access token
            var serverClient = httpClientFactory.CreateClient();

            var discoveryDocument = await serverClient.GetDiscoveryDocumentAsync("https://localhost:44332/");

            var tokenResponse = await serverClient.RequestClientCredentialsTokenAsync(
                new ClientCredentialsTokenRequest
                {
                    Address = discoveryDocument.TokenEndpoint,
                    ClientId = "client_id",
                    ClientSecret = "client_secret",

                    Scope = "ApiOne",
                });

            //retrive secret data
            var apiClient = httpClientFactory.CreateClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);
            var response = await apiClient.GetAsync("https://localhost:44344/secret");
            var content = await response.Content.ReadAsStringAsync();
            return Ok(new
            {
                access_token=tokenResponse.AccessToken,
                message=content
            });
        }
    }
}
