using System.Threading.Tasks;
using Compass.Security.Application.Services.Accounts.Commands.External;
using Compass.Security.Application.Services.Accounts.Commands.SignIn;
using Compass.Security.Application.Services.Accounts.Commands.SignOut;
using Compass.Security.Application.Services.Accounts.Commands.SignUp;
using Compass.Security.Application.Services.Accounts.Queries.Scheme;
using Compass.Security.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Compass.Security.Web.Controllers.Web
{
    public class AccountController : BaseWebController
    {

        private readonly SignInManager<User> _signInManager;

        public AccountController(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }
        
        public async Task<IActionResult> SignIn(string returnUrl)
        {
            if (User.Identity is not {IsAuthenticated: true})
            {
                var response = await Mediator.Send(new SchemeQuery());
                
                return View(new SignInCommand
                {
                    ReturnUrl = returnUrl,
                    ExternalLogins = response.Data
                });
            }
            
            if (!string.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
                
            return RedirectToAction("Index", "Home");
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInCommand command)
        {
            var response = await Mediator.Send(command);

            return response.Success switch
            {
                true when !string.IsNullOrEmpty(command.ReturnUrl) => Redirect(command.ReturnUrl),
                true => RedirectToAction("Index", "Home"),
                _ => View(command)
            };
        }
        
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLogin(string provider, string returnUrl)
        {
            var response = await Mediator.Send(new ExternalCommand {Provider = provider, ReturnUrl = returnUrl});

            var data = new ChallengeResult(provider, response.Data);
            
            return data;
        }
        
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            var response = await Mediator.Send(new CallbackCommand { ReturnUrl = returnUrl, RemoteError = remoteError });

            if (!response.Success)
            {
                TempData["message"] = response.Message;
                
                return View("SignIn", new SignInCommand
                {
                    ReturnUrl = returnUrl,
                    ExternalLogins = (await Mediator.Send(new SchemeQuery())).Data
                });
            }

            if(string.IsNullOrEmpty(returnUrl))
                return RedirectToAction("Index", "Home");
                
            return LocalRedirect(returnUrl);
        }
        
        public IActionResult SignUp()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignUpCommand command)
        {
            var response = await Mediator.Send(command);
            
            if(response.Success)
                TempData["message"] = response.Message;
            
            return response.Success switch
            {
                true when !string.IsNullOrEmpty(command.ReturnUrl) => Redirect(command.ReturnUrl),
                true => RedirectToAction("Verification"),
                _ => View(command)
            };
        }

        public IActionResult Recovery()
        {
            return View();
        }
        
        public IActionResult Verification()
        {
            return View();
        }
        
        public IActionResult TwoStep()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> SignOut(string returnUrl)
        {
            await Mediator.Send(new SignOutCommand());
            
            if(!string.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }
    }
}