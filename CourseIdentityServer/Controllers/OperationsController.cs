using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static CourseIdentityServer.Controllers.CookieJarAuthorizationHandler;

namespace CourseIdentityServer.Controllers
{
    public class OperationsController : Controller
    {
        private readonly IAuthorizationService authorizationService;

        public OperationsController(IAuthorizationService authorization)
        {
            authorizationService = authorization;
        }
        public async Task<IActionResult> Open()
        {
            var cookieJar = new CookieJar();//get cookie jar from db
            await authorizationService.AuthorizeAsync(User, cookieJar, CookieJarAuthOperations.Open);
            return View();
        }
    }
    public class CookieJarAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, CookieJar>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement,
            CookieJar cookieJar)
        {
            if (requirement.Name == CookieJarOperations.LOOK)
            {
                if (context.User.Identity.IsAuthenticated)
                {
                    context.Succeed(requirement);
                }
            }
            else if (requirement.Name == CookieJarOperations.COMENEAR)
            {
                if (context.User.HasClaim("Friend", "Good"))
                {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }

        public static class CookieJarAuthOperations
        {
            public static OperationAuthorizationRequirement Open = new OperationAuthorizationRequirement
            {
                Name = CookieJarOperations.OPEN
            };
        }

        public static class CookieJarOperations
        {
            public static string OPEN = "Open";
            public static string TAKECOOKIE = "TakeCookie";
            public static string COMENEAR = "ComeNear";
            public static string LOOK = "Look";
        }
    }

    public class CookieJar
    {
        public string Name { get; set; }
    }
}
