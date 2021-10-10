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
        
        public TwoFactorHandler(IIdentityService identityService, IMediator mediator)
        {
            _identityService = identityService;
            _mediator = mediator;
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
                    return ResponseDto.Fail("We sent a verification email to activate your account, please check your email.", false);
                case IdentityTypeEnum.IsLockedOut:
                    await _mediator.Publish(new SignInNotification() { Username = user.UserName }, cancellationToken);
                    return ResponseDto.Fail("It seems that you have exceeded the maximum number of attempts, please try again later.", false);
                case IdentityTypeEnum.RequiresTwoFactor:
                    return ResponseDto.Fail("Wrong token, please try again.", false);
                case IdentityTypeEnum.Failed:
                    return ResponseDto.Fail("The code is incorrect, please check, or generate a new authentication code.", false);
                default:
                    return ResponseDto.Fail("The code is incorrect, please check, or generate a new authentication code.", false);
            }
        }
    }
}