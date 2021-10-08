using Compass.Security.Application.Commons.Dtos;
using MediatR;

namespace Compass.Security.Application.Services.Accounts.Commands.Reset
{
    public class ResetCommand : IRequest<ResponseDto<bool>>
    {
        public string UserId { get; set; }

        public string Token { get; set; }

        public string ReturnUrl { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }
}