using System.Collections.Generic;
using AutoMapper;
using Compass.Security.Application.Commons.Dtos;
using Compass.Security.Application.Commons.Mapping;
using Compass.Security.Application.Services.Accounts.Commands.SignUp;
using Compass.Security.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authentication;

namespace Compass.Security.Application.Services.Accounts.Commands.SignIn
{
    public class SignInCommand : IRequest<ResponseDto<SignInCommand>>
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public string Captcha { get; set; }

        public bool IsOtp { get; set; }

        public string ReturnUrl { get; set; }
    }
}