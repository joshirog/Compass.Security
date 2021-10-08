using Compass.Security.Application.Commons.Dtos;
using MediatR;

namespace Compass.Security.Application.Services.Accounts.Commands.Resend
{
    public class ResendCommand : IRequest<ResponseDto<bool>>
    {
        public string UserId { get; set; }

        public string Email { get; set; }

        public string ReturnUrl { get; set; }
    }
}