using System;
using AutoMapper;
using Compass.Security.Application.Commons.Dtos;
using Compass.Security.Application.Commons.Mapping;
using Compass.Security.Domain.Entities;
using MediatR;

namespace Compass.Security.Application.Services.Accounts.Commands.SignIn
{
    public class SignInCommand : IRequest<ResponseDto<SignInCommand>>, IMapFrom<User>
    {
        public Guid Id { get; set; }
        
        public string Username { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public string Captcha { get; set; }

        public bool IsOtp { get; set; }

        public string ReturnUrl { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SignInCommand, User>()
                .ReverseMap();
        }
    }
}