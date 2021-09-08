using System;
using Compass.Security.Domain.Commons.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Compass.Security.Domain.Entities
{
    public class User : IdentityUser<Guid>, IBaseEntity
    {
        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }
    }
}