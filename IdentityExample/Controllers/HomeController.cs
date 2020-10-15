using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        static bool mailSent = default(bool);
        public HomeController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            configuration = config;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            // Login Functionality

            var user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                // sign in

                var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);
                if (signInResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        public IActionResult Register()
        {
            return View(); ;
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string email, string password)
        {
            // Register Functionality

            var user = new IdentityUser
            {
                UserName = username,
                Email = email
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                // generation of the email token
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var link = Url.Action(nameof(VerifyEmail), "Home", new { userId = user.Id, code },Request.Scheme,Request.Host.ToString());

                var emailResult = await SendConfirmationEmail(link, email);

                if (emailResult)
                {
                    return RedirectToAction("EmailConfirmation");
                }
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> VerifyEmail(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) return BadRequest();

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return View();
            }
            return BadRequest();
        }

        public IActionResult EmailConfirmation() => View();

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }


        private async Task<bool> SendConfirmationEmail(string link, string email)
        {
            var emailSend = default(bool);
            Exception exception = default(Exception);
            try
            {
                var getCredentials=configuration.GetSection("Email").Get<NetworkCredential>();
                var getSMTP = configuration.GetSection("Email").Get<SmtpClient>();
                NetworkCredential credential = new NetworkCredential();
                credential.UserName = "eduardo.ca.se.100@gmail.com";
                credential.Password = "L@locura1.10";
                // Command-line argument must be the SMTP host.
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Credentials = credential;
                // Specify the email sender.
                // Create a mailing address that includes a UTF8 character
                // in the display name.
                MailAddress from = new MailAddress("eduardo.cs.se.100@gmail.com",
                   "Mis Mecánicos" + (char)0xD8 + " EC",
                System.Text.Encoding.UTF8);
                // Set destinations for the email message.
                MailAddress to = new MailAddress(email);
                // Specify the message content.
                MailMessage message = new MailMessage(from, to);
                message.Body = "Da click en el siguiente Link para confirmar tu cueta" + System.Environment.NewLine + $"<a href=\"{link}\">Verify Email</a>";
                //// Include some non-ASCII characters in body and subject.
                //string someArrows = new string(new char[] { '\u2190', '\u2191', '\u2192', '\u2193' });
                //message.Body += Environment.NewLine + someArrows;
                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.Subject = "Email confirmation";
                message.SubjectEncoding = System.Text.Encoding.UTF8;
                // Set the method that is called back when the send operation ends.
                client.SendCompleted += new
                SendCompletedEventHandler(SendCompletedCallback);
                // The userState can be any object that allows your callback
                // method to identify this send operation.
                // For this example, the userToken is a string constant.
                await client.SendMailAsync(message);

                message.Dispose();
                Debug.WriteLine("Goodbye.");
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            finally
            {
                if (exception == null)
                {
                    emailSend = true;
                }
            }
            return emailSend;
        }

        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            String token = (string)e.UserState.ToString();

            if (e.Cancelled)
            {
                Debug.WriteLine("[{0}] Send canceled.", token);
            }
            if (e.Error != null)
            {
                Debug.WriteLine("[{0}] {1}", token, e.Error.ToString());
            }
            else
            {
                Debug.WriteLine("Message sent.");
            }
            mailSent = true;
        }
    }
}
