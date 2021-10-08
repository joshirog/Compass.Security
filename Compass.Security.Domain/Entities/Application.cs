using System;
using Compass.Security.Domain.Interfaces;
using OpenIddict.EntityFrameworkCore.Models;

namespace Compass.Security.Domain.Entities
{
    public class Application : OpenIddictEntityFrameworkCoreApplication<Guid, ApplicationAuthorization, ApplicationToken>, IBaseEntity
    {
        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }
    }
}