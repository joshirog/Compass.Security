using System;
using Compass.Security.Application.Commons.Dtos;
using MediatR;

namespace Compass.Security.Application.Services.Accounts.Commands.Otp
{
    public class OtpCommand : IRequest<ResponseDto<bool>>
    {
        public Guid UserId { get; set; }

        public string ReturnUrl { get; set; }
    }
}