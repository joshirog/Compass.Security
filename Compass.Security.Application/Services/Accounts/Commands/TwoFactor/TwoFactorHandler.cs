using System.Threading;
using System.Threading.Tasks;
using Compass.Security.Application.Commons.Constants;
using Compass.Security.Application.Commons.Dtos;
using Compass.Security.Application.Commons.Interfaces;
using Compass.Security.Application.Services.Accounts.Commands.SignIn;
using Compass.Security.Domain.Enums;
using MediatR;

namespace Compass.Security.Application.Services.Accounts.Commands.TwoFactor
{
    public class TwoFactorHandler : IRequestHandler<TwoFactorCommand, ResponseDto<bool>>
    {
        private readonly IIdentityService _identityService;
        private readonly IMediator _mediator;
        private readonly IUserNotificationRepository _userNotificationRepository;
        
        public TwoFactorHandler(IIdentityService identityService, IMediator mediator, IUserNotificationRepository userNotificationRepository)
        {
            _identityService = identityService;
            _mediator = mediator;
            _userNotificationRepository = userNotificationRepository;
        }
        
        public async Task<ResponseDto<bool>> Handle(TwoFactorCommand request, CancellationToken cancellationToken)
        {
            var code = $"{request.Code1}{request.Code2}{request.Code3}{request.Code4}{request.Code5}{request.Code6}";
            
            var (type, user) = await _identityService.TwoFactor(code);
            
            switch (type)
            {
                case IdentityTypeEnum.Succeeded:
                    return ResponseDto.Ok(ResponseConstant.MessageSuccess, true);
                case IdentityTypeEnum.IsNotAllowed:
                    return ResponseDto.Fail(ResponseConstant.MessageConfirm, false);
                case IdentityTypeEnum.IsLockedOut:
                    var notification = await _userNotificationRepository.GetFilterAsync(x =>
                        x.UserId.Equals(user.Id) &&
                        x.Type.Equals(NotificationTypeEnum.Locked));
                    if (notification.Counter < ConfigurationConstant.UserMaxEmail)
                        await _mediator.Publish(new SignInNotification() { Username = user.UserName }, cancellationToken);
                    return ResponseDto.Fail(ResponseConstant.MessageLockedAccount, false);
                case IdentityTypeEnum.RequiresTwoFactor:
                    return ResponseDto.Fail(ResponseConstant.MessageTwoFactorError, false);
                case IdentityTypeEnum.Failed:
                    return ResponseDto.Fail(ResponseConstant.MessageTwoFactorFail, false);
                default:
                    return ResponseDto.Fail(ResponseConstant.MessageTwoFactorFail, false);
            }
        }
    }
}