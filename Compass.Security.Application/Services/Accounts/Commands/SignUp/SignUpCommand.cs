using System;
using AutoMapper;
using Compass.Security.Application.Commons.Dtos;
using Compass.Security.Application.Commons.Mapping;
using Compass.Security.Domain.Commons.Enums;
using Compass.Security.Domain.Entities;
using MediatR;

namespace Compass.Security.Application.Services.Accounts.Commands.SignUp
{
    public class SignUpCommand : IRequest<ResponseDto<SignUpResponse>>, IMapFrom<User>
    {
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string ReturnUrl { get; set; }
        
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SignUpCommand, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.GetName(typeof(StatusEnum), StatusEnum.Active)));
        }
    }
}