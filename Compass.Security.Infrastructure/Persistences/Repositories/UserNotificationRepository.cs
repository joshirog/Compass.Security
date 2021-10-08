using Compass.Security.Application.Commons.Interfaces;
using Compass.Security.Domain.Entities;
using Compass.Security.Infrastructure.Persistences.Contexts;

namespace Compass.Security.Infrastructure.Persistences.Repositories
{
    public class UserNotificationRepository : BaseRepository<UserNotification>, IUserNotificationRepository
    {
        public UserNotificationRepository(ApplicationDbContext context) : base(context)
        {
            
        }
    }
}