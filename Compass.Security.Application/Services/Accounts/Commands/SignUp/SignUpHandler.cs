using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Compass.Security.Application.Commons.Constants;
using Compass.Security.Application.Commons.Dtos;
using Compass.Security.Application.Commons.Interfaces;
using Compass.Security.Domain.Entities;
using Compass.Security.Domain.Enums;
using MediatR;

namespace Compass.Security.Application.Services.Accounts.Commands.SignUp
{
    public class SignUpHandler : IRequestHandler<SignUpCommand, ResponseDto<SignUpResponse>>
    {
        private readonly IIdentityService _identityService;
        private readonly IUserNotificationRepository _userNotificationRepository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public SignUpHandler(IIdentityService identityService, IMapper mapper, IMediator mediator, IUserNotificationRepository userNotificationRepository)
        {
            _identityService = identityService;
            _mapper = mapper;
            _mediator = mediator;
            _userNotificationRepository = userNotificationRepository;
        }
        
        public async Task<ResponseDto<SignUpResponse>> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            var (isSuccess, user) = await _identityService.SignUp(_mapper.Map<User>(request), request.Password, new List<Claim>
            {
                new("firstname", request.Firstname),
                new("lastname", request.Lastname),
                new("avatar", ConfigurationConstant.Avatar, ClaimValueTypes.String)
            });

            
            foreach (var type in (NotificationTypeEnum[]) Enum.GetValues(typeof(NotificationTypeEnum)))
            {
                if (!type.Equals(NotificationTypeEnum.None))
                {
                    await _userNotificationRepository.InsertAsync(new UserNotification
                    {
                        UserId = user.Id,
                        Type = type,
                        Counter = 0,
                    });
                }
            }

            if (isSuccess)
                await _mediator.Publish(new SignUpNotification{ UserName = user.UserName }, cancellationToken);

            return isSuccess ? 
                ResponseDto.Ok(ResponseConstant.MessageSuccess, _mapper.Map<SignUpResponse>(user)) : 
                ResponseDto.Fail(ResponseConstant.MessageFail, new SignUpResponse());
        }
    }
}