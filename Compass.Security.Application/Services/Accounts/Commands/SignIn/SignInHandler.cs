using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Compass.Security.Application.Commons.Constants;
using Compass.Security.Application.Commons.Dtos;
using Compass.Security.Application.Commons.Interfaces;
using MediatR;

namespace Compass.Security.Application.Services.Accounts.Commands.SignIn
{
    public class SignInHandler: IRequestHandler<SignInCommand, ResponseDto<SignInResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;
        private readonly ICaptchaService _captchaService;

        public SignInHandler(IMapper mapper, IIdentityService identityService, ICaptchaService captchaService)
        {
            _mapper = mapper;
            _identityService = identityService;
            _captchaService = captchaService;
        }
        
        public async Task<ResponseDto<SignInResponse>> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            var isValid = await _captchaService.SiteVerify(request.Captcha);

            if (!isValid)
            {
                return ResponseDto.Fail(ResponseConstant.MessageFail, new SignInResponse());
            }

            var (isSuccess, user) = await _identityService.SignIn(request.Username, request.Password, request.RememberMe, true);
            
            return isSuccess ? 
                ResponseDto.Ok(ResponseConstant.MessageSuccess, _mapper.Map<SignInResponse>(user)) : 
                ResponseDto.Fail(ResponseConstant.MessageFail, new SignInResponse());
        }
    }
}