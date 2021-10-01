using Compass.Security.Application.Commons.Dtos;
using MediatR;

namespace Compass.Security.Application.Services.Accounts.Commands.SignUp
{
    public class SignUpCommand : IRequest<ResponseDto<SignUpResponse>>
    {
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public bool Agree { get; set; }

        public string ReturnUrl { get; set; }
    }
}