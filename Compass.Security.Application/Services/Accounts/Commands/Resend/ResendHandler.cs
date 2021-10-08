using System;
using System.Threading;
using System.Threading.Tasks;
using Compass.Security.Application.Commons.Constants;
using Compass.Security.Application.Commons.Dtos;
using Compass.Security.Application.Commons.Interfaces;
using Compass.Security.Application.Services.Accounts.Commands.SignUp;
using MediatR;

namespace Compass.Security.Application.Services.Accounts.Commands.Resend
{
    public class ResendHandler : IRequestHandler<ResendCommand,  ResponseDto<bool>>
    {
        private readonly IMediator _mediator;
        private readonly IUserRepository _userRepository;

        public ResendHandler(IMediator mediator, IUserRepository userRepository)
        {
            _mediator = mediator;
            _userRepository = userRepository;
        }
        
        public async Task<ResponseDto<bool>> Handle(ResendCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(Guid.Parse(request.UserId));
            
            await _mediator.Publish(new SignUpNotification{ UserName = user.UserName }, cancellationToken);

            return ResponseDto.Ok(ResponseConstant.MessageSuccess, true);
        }
    }
}