using AutoMapper;
using Compass.Security.Application.Commons.Mapping;
using Compass.Security.Domain.Entities;

namespace Compass.Security.Application.Services.Accounts.Commands.SignIn
{
    public class SignInResponse : IMapFrom<User>
    {
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, SignInResponse>();
        }
    }
}