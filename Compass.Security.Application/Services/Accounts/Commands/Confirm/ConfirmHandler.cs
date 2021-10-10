using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Compass.Security.Application.Commons.Constants;
using Compass.Security.Application.Commons.Dtos;
using Compass.Security.Application.Commons.Interfaces;
using MediatR;
using Microsoft.AspNetCore.WebUtilities;

namespace Compass.Security.Application.Services.Accounts.Commands.Confirm
{
    public class ConfirmHandler : IRequestHandler<ConfirmCommand, ResponseDto<bool>>
    {
        private readonly IMediator _mediator;
        private readonly IIdentityService _identityService;

        public ConfirmHandler(IIdentityService identityService, IMediator mediator)
        {
            _identityService = identityService;
            _mediator = mediator;
        }
        
        public async Task<ResponseDto<bool>> Handle(ConfirmCommand request, CancellationToken cancellationToken)
        {
            var tokenDecodedBytes = WebEncoders.Base64UrlDecode(request.Token);
            var tokenDecoded = Encoding.UTF8.GetString(tokenDecodedBytes);
            
            var result = await _identityService.ConfirmEmail(request.UserId, tokenDecoded);

            if (!result) 
                return ResponseDto.Fail(ResponseConstant.MessageFail, false);
            
            await _mediator.Publish(new ConfirmNotification{ UserId = request.UserId }, cancellationToken);

            return ResponseDto.Ok(ResponseConstant.MessageSuccessConfirm, true);
        }
    }
}