using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Compass.Security.Application.Commons.Constants;
using Compass.Security.Application.Commons.Dtos;
using Compass.Security.Application.Commons.Interfaces;
using Compass.Security.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;

namespace Compass.Security.Application.Services.Accounts.Commands.Recovery
{
    public class RecoveryHandler : IRequestHandler<RecoveryCommand, ResponseDto<bool>>
    {
        private readonly IMediator _mediator;
        private readonly IIdentityService _identityService;
        private readonly IHttpContextAccessor _accessor;
        private readonly IUserRepository _userRepository;
        private readonly IUserNotificationRepository _userNotificationRepository;

        public RecoveryHandler(IMediator mediator, IUserRepository userRepository, IHttpContextAccessor accessor, IIdentityService identityService, IUserNotificationRepository userNotificationRepository)
        {
            _mediator = mediator;
            _userRepository = userRepository;
            _accessor = accessor;
            _identityService = identityService;
            _userNotificationRepository = userNotificationRepository;
        }
        
        public async Task<ResponseDto<bool>> Handle(RecoveryCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetFilterAsync(x => x.Email.Equals(request.EmailOrPhone) || x.PhoneNumber.Equals(request.EmailOrPhone));
            
            if(user is null)
                return ResponseDto.Fail("We have sent the email instructions to follow and recover the password.", false);

            var notification = await _userNotificationRepository.GetFilterAsync(x => 
                x.UserId.Equals(user.Id) &&
                x.Type.Equals(NotificationTypeEnum.Reset));
            
            if(notification.Counter > ConfigurationConstant.UserMaxEmail)
                return ResponseDto.Fail("You have exceeded the maximum number of daily notifications, please try again later.", false);
            
            var claims = await _identityService.GetClaims(user);
            var firstname = claims.Where(x => x.Type.Equals("firstname")).Select(x => x.Value).FirstOrDefault();

            var token = await _identityService.TokenPassword(user);
            
            var tokenEncodedBytes = Encoding.UTF8.GetBytes(token);
            var tokenEncoded = WebEncoders.Base64UrlEncode(tokenEncodedBytes);
            
            var callback = $"{_accessor.HttpContext?.Request.Scheme}://{_accessor.HttpContext?.Request.Host}/account/reset/{user.Id}?token={tokenEncoded}&returnUrl={request.ReturnUrl}";
            
            Console.WriteLine(callback);
            
            await _mediator.Publish(new RecoveryNotification
            {
                UserId = user.Id,
                Name = firstname,
                Email = user.Email,
                Callback = callback
            }, cancellationToken);
    
            return ResponseDto.Ok(ResponseConstant.MessageChangePassword, true);
        }
    }
}