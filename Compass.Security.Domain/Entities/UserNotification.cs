using System;
using Compass.Security.Domain.Enums;
using Compass.Security.Domain.Interfaces;

namespace Compass.Security.Domain.Entities
{
    public class UserNotification : IBaseEntity
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public NotificationTypeEnum Type { get; set; }

        public int Counter { get; set; }
        
        public string Status { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public string CreatedBy { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
        
        public string UpdatedBy { get; set; }
        

        public User User { get; set; }
    }
}