using System;
using System.Threading;
using System.Threading.Tasks;
using Compass.Security.Application.Commons.Constants;
using Compass.Security.Application.Commons.Dtos;
using Compass.Security.Application.Commons.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Compass.Security.Application.Services.Accounts.Commands.External
{
    public class ExternalHandler : IRequestHandler<ExternalCommand, ResponseDto<AuthenticationProperties>>
    {
        private readonly IIdentityService _identityService;
        private readonly IHttpContextAccessor _accessor;

        public ExternalHandler(IIdentityService identityService, IHttpContextAccessor accessor)
        {
            _identityService = identityService;
            _accessor = accessor;
        }
        
        public async Task<ResponseDto<AuthenticationProperties>> Handle(ExternalCommand request, CancellationToken cancellationToken)
        {
            var redirectUrl = $"{_accessor.HttpContext?.Request.Scheme}://{_accessor.HttpContext?.Request.Host}/Account/ExternalLoginCallback?returnUrl={request.ReturnUrl}";
            
            Console.WriteLine(redirectUrl);
            
            await Task.CompletedTask;
            
            var properties = _identityService.Properties(request.Provider, redirectUrl);
            
            return properties is not null ? 
                ResponseDto.Ok(ResponseConstant.MessageSuccess, properties) : 
                ResponseDto.Fail(ResponseConstant.MessageFail, new AuthenticationProperties());
        }
    }
}