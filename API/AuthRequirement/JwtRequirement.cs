using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace API.AuthRequirement
{
    public class JwtRequirement : IAuthorizationRequirement{}

    public class JwtRequirementHandler : AuthorizationHandler<JwtRequirement>
    {
        private readonly HttpClient client;
        private readonly HttpContext httpContext;
        public JwtRequirementHandler(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            client = httpClientFactory.CreateClient();
            httpContext = httpContextAccessor.HttpContext;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, JwtRequirement requirement)
        {
            if(httpContext.Request.Headers.TryGetValue("Authorization",out var authHeader))
            {
                var accessToken = authHeader.ToString().Split(' ')[1];

                var response = await client.GetAsync($"https://localhost:44352/oauth/validate?access_token={accessToken}");

                if (response.IsSuccessStatusCode)
                {
                    context.Succeed(requirement);
                }
            }
        }
    }
}
