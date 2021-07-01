using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyClient.Controllers
{
    public class HomeController:Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        public HomeController(IHttpClientFactory clientFactory)
        {
            httpClientFactory = clientFactory;
        }
        public IActionResult Index()
        {
            return View();
        }
        
        [Authorize]
        public async Task<IActionResult> Secret()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var idToken = await HttpContext.GetTokenAsync("id_token");
            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");

            var claims = User.Claims.ToList();
            var _accessToken = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
            var _idToken = new JwtSecurityTokenHandler().ReadJwtToken(idToken);

            var result = await GetSecret(accessToken);
            
            return View();
        }

        public async Task<string> GetSecret(string accessToken)
        {
            var apiClient = httpClientFactory.CreateClient();
            apiClient.SetBearerToken(accessToken);
            var response = await apiClient.GetAsync("https://localhost:44344/secret");
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }
    }
}
