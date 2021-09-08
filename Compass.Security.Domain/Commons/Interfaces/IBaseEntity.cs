using System;

namespace Compass.Security.Domain.Commons.Interfaces
{
    public interface IBaseEntity
    {
        public Guid Id { get; set; }

        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }
    }
}