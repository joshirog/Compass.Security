using System.Collections.Generic;
using Compass.Security.Application.Commons.Dtos;
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

        public string ReturnUrl { get; set; }
    }
}