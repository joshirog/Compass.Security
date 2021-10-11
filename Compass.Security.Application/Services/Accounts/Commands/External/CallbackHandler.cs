using System.Threading;
using System.Threading.Tasks;
using Compass.Security.Application.Commons.Constants;
using Compass.Security.Application.Commons.Dtos;
using Compass.Security.Application.Commons.Interfaces;
using Compass.Security.Application.Services.Accounts.Commands.Confirm;
using MediatR;

namespace Compass.Security.Application.Services.Accounts.Commands.External
{
    public class CallbackHandler : IRequestHandler<CallbackCommand, ResponseDto<bool>>
    {
        private readonly IIdentityService _identityService;
        private readonly IMediator _mediator;
        
        public CallbackHandler(IIdentityService identityService, IMediator mediator)
        {
            _identityService = identityService;
            _mediator = mediator;
        }
        
        public async Task<ResponseDto<bool>> Handle(CallbackCommand request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.RemoteError))
                return ResponseDto.Fail($"{ResponseConstant.MessageErrorProvider} : {request.RemoteError}", false);

            var (isSuccess, user) = await _identityService.CallBack();
            
            if (!isSuccess) 
                return ResponseDto.Fail(ResponseConstant.MessageFail, false);
            
            if(user is not null)
                await _mediator.Publish(new ConfirmNotification{ UserId = user.Id.ToString() }, cancellationToken);

            return ResponseDto.Ok(ResponseConstant.MessageSuccessConfirm, true);
        }
    }
}