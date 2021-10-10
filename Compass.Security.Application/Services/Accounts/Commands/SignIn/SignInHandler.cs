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

        public SignInHandler(IIdentityService identityService, ICaptchaService captchaService, IMediator mediator, IMapper mapper)
        {
            _identityService = identityService;
            _captchaService = captchaService;
            _mediator = mediator;
            _mapper = mapper;
        }
        
        public async Task<ResponseDto<SignInCommand>> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            var isValid = await _captchaService.SiteVerify(request.Captcha);

            if (!isValid)
            {
                return ResponseDto.Fail(ResponseConstant.MessageFail, new SignInCommand());
            }

            var (type, user) = await _identityService.SignIn(request.Username, request.Password, request.RememberMe, true);

            var response = _mapper.Map<SignInCommand>(user);
            
            switch (type)
            {
                case IdentityTypeEnum.Succeeded:
                    return ResponseDto.Ok(ResponseConstant.MessageSuccess, response);
                case IdentityTypeEnum.IsNotAllowed:
                    return ResponseDto.Fail("We sent a verification email to activate your account, please check your email.", response);
                case IdentityTypeEnum.IsLockedOut:
                    await _mediator.Publish(new SignInNotification() { Username = user.UserName }, cancellationToken);
                    return ResponseDto.Fail("It seems that you have exceeded the maximum number of attempts, please try again later.", response);
                case IdentityTypeEnum.RequiresTwoFactor:
                    await _mediator.Publish(new OtpNotification { UserId = user.Id, ReturnUrl = request.ReturnUrl }, cancellationToken);
                    response.IsOtp = true;
                    return ResponseDto.Ok(ResponseConstant.MessageSuccess, response);
                case IdentityTypeEnum.Failed:
                    return ResponseDto.Fail("Incorrect email or password, please check and try again.", response);
                default:
                    return ResponseDto.Fail("Incorrect email or password, please check and try again.", response);
            }
        }
    }
}