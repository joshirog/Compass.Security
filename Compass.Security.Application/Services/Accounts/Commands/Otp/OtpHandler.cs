using System.Threading;
using System.Threading.Tasks;
using Compass.Security.Application.Commons.Constants;
using Compass.Security.Application.Commons.Dtos;
using MediatR;

namespace Compass.Security.Application.Services.Accounts.Commands.Otp
{
    public class OtpHandler : IRequestHandler<OtpCommand, ResponseDto<bool>>
    {
        private readonly IMediator _mediator;

        public OtpHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        public async Task<ResponseDto<bool>> Handle(OtpCommand request, CancellationToken cancellationToken)
        {
            await _mediator.Publish(new OtpNotification { UserId = request.UserId, ReturnUrl = request.ReturnUrl}, cancellationToken);

            return ResponseDto.Ok(ResponseConstant.MessageSuccessOpt, true);
        }
    }
}