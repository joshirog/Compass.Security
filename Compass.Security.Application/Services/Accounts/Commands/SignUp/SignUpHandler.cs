using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Compass.Security.Application.Commons.Dtos;
using Compass.Security.Application.Commons.Interfaces;
using Compass.Security.Domain.Entities;
using MediatR;

namespace Compass.Security.Application.Services.Accounts.Commands.SignUp
{
    public class SignUpHandler : IRequestHandler<SignUpCommand, ResponseDto<SignUpResponse>>
    {
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public SignUpHandler(IIdentityService identityService, IMapper mapper)
        {
            _identityService = identityService;
            _mapper = mapper;
        }
        
        public async Task<ResponseDto<SignUpResponse>> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            var (isSuccess, user) = await _identityService.SignUp(_mapper.Map<User>(request), request.Password, new List<Claim>
            {
                new("firstname", request.Firstname),
                new("lastname", request.Lastname),
            });

            return isSuccess ? 
                ResponseDto.Ok("", _mapper.Map<SignUpResponse>(user)) : 
                ResponseDto.Fail("", new SignUpResponse());
        }
    }
}