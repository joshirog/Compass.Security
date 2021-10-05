using System.Threading;
using System.Threading.Tasks;
using Compass.Security.Application.Commons.Constants;
using Compass.Security.Application.Commons.Dtos;
using Compass.Security.Application.Commons.Interfaces;
using MediatR;

namespace Compass.Security.Application.Services.Accounts.Commands.External
{
    public class CallbackHandler : IRequestHandler<CallbackCommand, ResponseDto<bool>>
    {
        private readonly IIdentityService _identityService;
        
        public CallbackHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        
        public async Task<ResponseDto<bool>> Handle(CallbackCommand request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.RemoteError))
                return ResponseDto.Fail($"{ResponseConstant.MessageErrorProvider} : {request.RemoteError}", false);

            var result = await _identityService.CallBack();
            
            return result ? 
                ResponseDto.Ok(ResponseConstant.MessageSuccess, true) : 
                ResponseDto.Fail(ResponseConstant.MessageFail, false);
        }
    }
}