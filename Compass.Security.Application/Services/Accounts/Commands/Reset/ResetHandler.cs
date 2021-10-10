using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Compass.Security.Application.Commons.Constants;
using Compass.Security.Application.Commons.Dtos;
using Compass.Security.Application.Commons.Interfaces;
using Compass.Security.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.WebUtilities;

namespace Compass.Security.Application.Services.Accounts.Commands.Reset
{
    public class ResetHandler : IRequestHandler<ResetCommand, ResponseDto<bool>>
    {
        private readonly IMediator _mediator;
        private readonly IIdentityService _identityService;
        private readonly IUserNotificationRepository _userNotificationRepository;
        
        public ResetHandler(IMediator mediator, IIdentityService identityService, IUserNotificationRepository userNotificationRepository)
        {
            _mediator = mediator;
            _identityService = identityService;
            _userNotificationRepository = userNotificationRepository;
        }
        
        public async Task<ResponseDto<bool>> Handle(ResetCommand request, CancellationToken cancellationToken)
        {
            var notification = await _userNotificationRepository.GetFilterAsync(x =>
                x.UserId.Equals(request.UserId) &&
                x.Type.Equals(NotificationTypeEnum.Resend));
            
            if (notification.Counter >= ConfigurationConstant.UserMaxEmail)
                return ResponseDto.Fail(ResponseConstant.MessageMaxNotification, false);
            
            var tokenDecodedBytes = WebEncoders.Base64UrlDecode(request.Token);
            var tokenDecoded = Encoding.UTF8.GetString(tokenDecodedBytes);
            
            var result = await _identityService.ResetPassword(request.UserId, tokenDecoded, request.Password);
            
            if(result)
                await _mediator.Publish(new ResetNotification{ UserId = request.UserId }, cancellationToken);
            
            return result ? 
                ResponseDto.Ok(ResponseConstant.MessageSuccessPassword, true) : 
                ResponseDto.Fail(ResponseConstant.MessageFail, false);
        }
    }
}