using AutoMapper;
using Compass.Security.Application.Commons.Mapping;
using Compass.Security.Domain.Entities;

namespace Compass.Security.Application.Services.Accounts.Commands.SignUp
{
    public class SignUpResponse : IMapFrom<User>
    {
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, SignUpResponse>();
        }
    }
}