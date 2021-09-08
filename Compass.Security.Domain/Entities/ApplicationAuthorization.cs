using System;
using OpenIddict.EntityFrameworkCore.Models;

namespace Compass.Security.Domain.Entities
{
    public class ApplicationAuthorization : OpenIddictEntityFrameworkCoreAuthorization<Guid, Application, ApplicationToken>
    {
        public Guid ApplicationId { get; set; }
    }
}