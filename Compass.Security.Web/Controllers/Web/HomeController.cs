using Microsoft.AspNetCore.Mvc;

namespace Compass.Security.Web.Controllers.Web
{
    public class HomeController : BaseWebController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}