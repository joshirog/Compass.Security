using Compass.Security.Application.Commons.Dtos;
using MediatR;

namespace Compass.Security.Application.Services.Accounts.Commands.Confirm
{
    public class ConfirmCommand : IRequest<ResponseDto<bool>>
    {
        public string UserId { get; set; }

        public string Token { get; set; }

        public string ReturnUrl { get; set; }
    }
}