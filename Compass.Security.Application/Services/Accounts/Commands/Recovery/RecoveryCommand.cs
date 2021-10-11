using Compass.Security.Application.Commons.Dtos;
using MediatR;

namespace Compass.Security.Application.Services.Accounts.Commands.Recovery
{
    public class RecoveryCommand : IRequest<ResponseDto<bool>>
    {
        public string EmailOrPhone { get; set; }

        public string ReturnUrl { get; set; }
    }
}