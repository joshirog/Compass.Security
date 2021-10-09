using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Compass.Security.Application.Commons.Constants;
using Compass.Security.Application.Commons.Dtos;
using Compass.Security.Application.Commons.Interfaces;
using Compass.Security.Application.Services.Accounts.Commands.Otp;
using MediatR;

namespace Compass.Security.Application.Services.Accounts.Commands.SignIn
{
    public class SignInHandler: IRequestHandler<SignInCommand, ResponseDto<SignInCommand>>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IIdentityService _identityService;
        private readonly ICaptchaService _captchaService;

        public SignInHandler(IMapper mapper, IIdentityService identityService, ICaptchaService captchaService, IMediator mediator)
        {
            _mapper = mapper;
            _identityService = identityService;
            _captchaService = captchaService;
            _mediator = mediator;
        }
        
        public async Task<ResponseDto<SignInCommand>> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            var isValid = await _captchaService.SiteVerify(request.Captcha);

            if (!isValid)
            {
                return ResponseDto.Fail(ResponseConstant.MessageFail, new SignInCommand());
            }

            var (isSuccess, isOtp, user) = await _identityService.SignIn(request.Username, request.Password, request.RememberMe, true);
            
            var response = _mapper.Map<SignInCommand>(user);
            response.IsOtp = isOtp;

            if (response.IsOtp)
                await _mediator.Publish(new OtpNotification { UserId = user.Id, ReturnUrl = request.ReturnUrl }, cancellationToken);
            
            return isSuccess ? 
                ResponseDto.Ok(ResponseConstant.MessageSuccess, request) : 
                ResponseDto.Fail(ResponseConstant.MessageFail, request);
        }
    }
}