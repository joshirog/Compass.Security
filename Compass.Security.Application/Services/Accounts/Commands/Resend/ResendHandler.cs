using System;
using System.Threading;
using System.Threading.Tasks;
using Compass.Security.Application.Commons.Constants;
using Compass.Security.Application.Commons.Dtos;
using Compass.Security.Application.Commons.Interfaces;
using Compass.Security.Application.Services.Accounts.Commands.SignUp;
using Compass.Security.Domain.Enums;
using MediatR;

namespace Compass.Security.Application.Services.Accounts.Commands.Resend
{
    public class ResendHandler : IRequestHandler<ResendCommand,  ResponseDto<bool>>
    {
        private readonly IMediator _mediator;
        private readonly IUserRepository _userRepository;
        private readonly IUserNotificationRepository _userNotificationRepository;

        public ResendHandler(IMediator mediator, IUserRepository userRepository, IUserNotificationRepository userNotificationRepository)
        {
            _mediator = mediator;
            _userRepository = userRepository;
            _userNotificationRepository = userNotificationRepository;
        }
        
        public async Task<ResponseDto<bool>> Handle(ResendCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(Guid.Parse(request.UserId));

            var notification = await _userNotificationRepository.GetFilterAsync(x =>
                x.UserId.Equals(user.Id) &&
                x.Type.Equals(NotificationTypeEnum.Resend));

            if (notification.Counter < ConfigurationConstant.UserMaxEmail)
                await _mediator.Publish(new SignUpNotification{ UserName = user.UserName }, cancellationToken);

            return ResponseDto.Ok(ResponseConstant.MessageConfirm, true);
        }
    }
}