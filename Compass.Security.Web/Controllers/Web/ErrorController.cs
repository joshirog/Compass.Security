using Microsoft.AspNetCore.Mvc;

namespace Compass.Security.Web.Controllers.Web
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode:int}")]
        public IActionResult Index(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Sorry, the page you're looking for cannot be found.";
                    return View("NotFound");
                case 500:
                    ViewBag.ErrorMessage = "The server encountered an internal error or misconfiguration and was unable to complete your request.";
                    return View("InternalError");
            }
            
            ViewBag.ErrorMessage = "The server encountered an internal error or misconfiguration and was unable to complete your request.";
            return View("InternalError");
        }
    }
}