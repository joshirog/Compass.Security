using System.Threading.Tasks;
using Compass.Security.Application.Services.Accounts.Commands.SignUp;
using Microsoft.AspNetCore.Mvc;

namespace Compass.Security.Web.Controllers.Web
{
    public class AccountController : BaseWebController
    {
        public IActionResult SignIn()
        {
            return View();
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
                true => RedirectToAction("Index", "Home"),
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
    }
}