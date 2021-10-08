using Compass.Security.Application.Commons.Interfaces;
using Compass.Security.Domain.Entities;
using Compass.Security.Infrastructure.Persistences.Contexts;

namespace Compass.Security.Infrastructure.Persistences.Repositories
{
    public class NotificationLogRepository : BaseRepository<NotificationLog>, INotificationLogRepository
    {
        public NotificationLogRepository(ApplicationDbContext context) : base(context)
        {
            
        }
    }
}