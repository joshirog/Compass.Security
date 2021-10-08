using System.Threading;
using System.Threading.Tasks;
using Compass.Security.Application.Commons.Constants;
using Compass.Security.Application.Commons.Dtos;
using Compass.Security.Application.Services.Accounts.Commands.SignUp;
using MediatR;

namespace Compass.Security.Application.Services.Accounts.Commands.Resend
{
    public class ResendHandler : IRequestHandler<ResendCommand,  ResponseDto<bool>>
    {
        private readonly IMediator _mediator;

        public ResendHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        public async Task<ResponseDto<bool>> Handle(ResendCommand request, CancellationToken cancellationToken)
        {
            await _mediator.Publish(new SignUpNotification{ UserName = request.Email }, cancellationToken);

            return ResponseDto.Ok(ResponseConstant.MessageSuccess, true);
        }
    }
}