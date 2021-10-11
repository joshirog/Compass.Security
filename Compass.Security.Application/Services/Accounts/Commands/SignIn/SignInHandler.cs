using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Compass.Security.Application.Commons.Constants;
using Compass.Security.Application.Commons.Dtos;
using Compass.Security.Application.Commons.Interfaces;
using Compass.Security.Application.Services.Accounts.Commands.Otp;
using Compass.Security.Domain.Enums;
using MediatR;

namespace Compass.Security.Application.Services.Accounts.Commands.SignIn
{
    public class SignInHandler : IRequestHandler<SignInCommand, ResponseDto<SignInCommand>>
    {
        private readonly IMediator _mediator;
        private readonly IIdentityService _identityService;
        private readonly ICaptchaService _captchaService;
        private readonly IMapper _mapper;
        private readonly IUserNotificationRepository _userNotificationRepository;

        public SignInHandler(IIdentityService identityService, ICaptchaService captchaService, IMediator mediator, IMapper mapper, IUserNotificationRepository userNotificationRepository)
        {
            _identityService = identityService;
            _captchaService = captchaService;
            _mediator = mediator;
            _mapper = mapper;
            _userNotificationRepository = userNotificationRepository;
        }
        
        public async Task<ResponseDto<SignInCommand>> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            var isValid = await _captchaService.SiteVerify(request.Captcha);

            if (!isValid)
                return ResponseDto.Fail(ResponseConstant.MessageFail, new SignInCommand());

            var (type, user) = await _identityService.SignIn(request.Username, request.Password, request.RememberMe, true);

            var response = _mapper.Map<SignInCommand>(user);
            
            switch (type)
            {
                case IdentityTypeEnum.Succeeded:
                    return ResponseDto.Ok(ResponseConstant.MessageSuccess, response);
                case IdentityTypeEnum.IsNotAllowed:
                    return ResponseDto.Fail(ResponseConstant.MessageConfirm, response);
                case IdentityTypeEnum.IsLockedOut:
                    var notification = await _userNotificationRepository.GetFilterAsync(x =>
                        x.UserId.Equals(user.Id) &&
                        x.Type.Equals(NotificationTypeEnum.Locked));
                    if (notification.Counter < ConfigurationConstant.UserMaxEmail)
                        await _mediator.Publish(new SignInNotification() { Username = user.UserName }, cancellationToken);
                    return ResponseDto.Fail(ResponseConstant.MessageLockedAccount, response);
                case IdentityTypeEnum.RequiresTwoFactor:
                    await _mediator.Publish(new OtpNotification { UserId = user.Id, ReturnUrl = request.ReturnUrl }, cancellationToken);
                    response.IsOtp = true;
                    return ResponseDto.Ok(ResponseConstant.MessageSuccess, response);
                case IdentityTypeEnum.Failed:
                    return ResponseDto.Fail(ResponseConstant.MessageSignInFail, response);
                default:
                    return ResponseDto.Fail(ResponseConstant.MessageSignInFail, response);
            }
        }
    }
}