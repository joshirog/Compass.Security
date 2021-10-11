using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Compass.Security.Web.Controllers.Web
{
    public class BaseWebController : Controller
    {
        private ISender _mediator;
        
        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>();
    }
}