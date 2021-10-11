using Compass.Security.Application.Commons.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authentication;

namespace Compass.Security.Application.Services.Accounts.Commands.External
{
    public class ExternalCommand : IRequest<ResponseDto<AuthenticationProperties>>
    {
        public string Provider { get; set; }

        public string ReturnUrl { get; set; }
    }
}