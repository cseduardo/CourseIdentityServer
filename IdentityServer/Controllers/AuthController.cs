using IdentityServer.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public AuthController(SignInManager<IdentityUser> signIn,UserManager<IdentityUser> user)
        {
            userManager = user;
            signInManager = signIn;
        }

        [HttpGet]
        public IActionResult SignIn(string returnUrl)
        {
            var model = new SignIn { ReturnUrl = returnUrl };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignIn register)
        {
            if (!ModelState.IsValid)
            {
                return View(register);
            }
            var user = new IdentityUser(register.Username);
            var result = await userManager.CreateAsync(user, register.Password);
            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, default(bool));
                return Redirect(register.ReturnUrl);
            }
            else if (result.Errors.Any())
            {

            }
            return View();
        }
        
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new Login { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            //check if the model is valid
            var resultPassword = await signInManager.PasswordSignInAsync(login.Username, login.Password, default(bool), default(bool));

            if (resultPassword.Succeeded)
            {
                return Redirect(login.ReturnUrl);
            }else if (resultPassword.IsLockedOut)
            {

            }
            return View();
        }
    }
}
