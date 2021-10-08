using System.Threading;
using System.Threading.Tasks;
using Compass.Security.Application.Commons.Constants;
using Compass.Security.Application.Commons.Dtos;
using Compass.Security.Application.Commons.Interfaces;
using MediatR;

namespace Compass.Security.Application.Services.Accounts.Commands.TwoFactor
{
    public class TwoFactorHandler : IRequestHandler<TwoFactorCommand, ResponseDto<bool>>
    {
        private readonly IIdentityService _identityService;
        
        public TwoFactorHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        
        public async Task<ResponseDto<bool>> Handle(TwoFactorCommand request, CancellationToken cancellationToken)
        {
            var code = $"{request.Code1}{request.Code2}{request.Code3}{request.Code4}{request.Code5}{request.Code6}";
            
            var result = await _identityService.TwoFactor(code);
            
            return result ? 
                ResponseDto.Ok(ResponseConstant.MessageSuccess, true) : 
                ResponseDto.Fail(ResponseConstant.MessageFail, false);
        }
    }
}