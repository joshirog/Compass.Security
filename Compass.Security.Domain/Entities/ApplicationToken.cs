using System;
using OpenIddict.EntityFrameworkCore.Models;

namespace Compass.Security.Domain.Entities
{
    public class ApplicationToken : OpenIddictEntityFrameworkCoreToken<Guid, Application, ApplicationAuthorization>
    {
        public Guid ApplicationId { get; set; }

        public Guid AuthorizationId { get; set; }
    }
}