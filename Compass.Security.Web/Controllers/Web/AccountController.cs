using Microsoft.AspNetCore.Mvc;

namespace Compass.Security.Web.Controllers.Web
{
    public class AccountController : Controller
    {
        public IActionResult SignIn()
        {
            return View();
        }
        
        public IActionResult SignUp()
        {
            return View();
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