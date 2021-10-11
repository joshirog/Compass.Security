using Compass.Security.Application.Commons.Dtos;
using MediatR;

namespace Compass.Security.Application.Services.Accounts.Commands.External
{
    public class CallbackCommand : IRequest<ResponseDto<bool>>
    {
        public string ReturnUrl { get; set; }

        public string RemoteError { get; set; }
    }
}