using System;
using Compass.Security.Domain.Interfaces;

namespace Compass.Security.Domain.Entities
{
    public class Blacklist : IBaseEntity
    {
        public Guid Id { get; set; }
        
        public string Type { get; set; }
        
        public string Status { get; set; }

        public string Value { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public string CreatedBy { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
        
        public string UpdatedBy { get; set; }
    }
}