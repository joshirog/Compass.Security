using Compass.Security.Application.Commons.Dtos;
using MediatR;

namespace Compass.Security.Application.Services.Accounts.Commands.Otp
{
    public class OtpCommand : IRequest<ResponseDto<bool>>
    {
        public string UserId { get; set; }

        public string ReturnUrl { get; set; }
    }
}