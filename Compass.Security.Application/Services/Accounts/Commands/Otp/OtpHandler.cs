using System.Threading;
using System.Threading.Tasks;
using Compass.Security.Application.Commons.Constants;
using Compass.Security.Application.Commons.Dtos;
using Compass.Security.Application.Commons.Interfaces;
using Compass.Security.Domain.Enums;
using MediatR;

namespace Compass.Security.Application.Services.Accounts.Commands.Otp
{
    public class OtpHandler : IRequestHandler<OtpCommand, ResponseDto<bool>>
    {
        private readonly IMediator _mediator;
        private readonly IUserNotificationRepository _userNotificationRepository;

        public OtpHandler(IMediator mediator, IUserNotificationRepository userNotificationRepository)
        {
            _mediator = mediator;
            _userNotificationRepository = userNotificationRepository;
        }
        
        public async Task<ResponseDto<bool>> Handle(OtpCommand request, CancellationToken cancellationToken)
        {
            var notification = await _userNotificationRepository.GetFilterAsync(x =>
                x.UserId.Equals(request.UserId) &&
                x.Type.Equals(NotificationTypeEnum.TwoFactor));

            if (notification.Counter >= ConfigurationConstant.UserMaxEmail)
                return ResponseDto.Fail(ResponseConstant.MessageMaxNotification, false);
            
            await _mediator.Publish(new OtpNotification { UserId = request.UserId, ReturnUrl = request.ReturnUrl}, cancellationToken);

            return ResponseDto.Ok(ResponseConstant.MessageSuccessOpt, true);
        }
    }
}