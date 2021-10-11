using Compass.Security.Application.Commons.Interfaces;
using Compass.Security.Domain.Entities;
using Compass.Security.Infrastructure.Persistences.Contexts;

namespace Compass.Security.Infrastructure.Persistences.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
            
        }
    }
}