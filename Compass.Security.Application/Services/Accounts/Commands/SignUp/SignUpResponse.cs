using AutoMapper;
using Compass.Security.Application.Commons.Mapping;
using Compass.Security.Domain.Entities;

namespace Compass.Security.Application.Services.Accounts.Commands.SignUp
{
    public class SignUpResponse : IMapFrom<User>
    {
        public string UserId { get; set; }

        public string Email { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, SignUpResponse>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));
        }
    }
}