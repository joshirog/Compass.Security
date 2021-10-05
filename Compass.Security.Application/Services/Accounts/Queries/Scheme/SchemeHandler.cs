using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Compass.Security.Application.Commons.Constants;
using Compass.Security.Application.Commons.Dtos;
using Compass.Security.Application.Commons.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authentication;

namespace Compass.Security.Application.Services.Accounts.Queries.Scheme
{
    public class SchemeHandler : IRequestHandler<SchemeQuery, ResponseDto<List<AuthenticationScheme>>>
    {
        private readonly IIdentityService _identityService;
        
        public SchemeHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        
        public async Task<ResponseDto<List<AuthenticationScheme>>> Handle(SchemeQuery request, CancellationToken cancellationToken)
        {
            var schemes = await _identityService.Schemes();
            
            return schemes is not null ? 
                ResponseDto.Ok(ResponseConstant.MessageSuccess, schemes) : 
                ResponseDto.Fail(ResponseConstant.MessageFail, new List<AuthenticationScheme>());
        }
    }
}