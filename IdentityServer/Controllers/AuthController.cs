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
            return View(new Register { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(Register register)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var user = new IdentityUser(register.Username);
            var result = await userManager.CreateAsync(user, "password");
            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, default(bool));
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
