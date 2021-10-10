using System;
using Compass.Security.Application.Commons.Dtos;
using MediatR;

namespace Compass.Security.Application.Services.Accounts.Commands.TwoFactor
{
    public class TwoFactorCommand: IRequest<ResponseDto<bool>>
    {
        public Guid Id { get; set; }
        
        public string Code1 { get; set; }
        
        public string Code2 { get; set; }
        
        public string Code3 { get; set; }
        
        public string Code4 { get; set; }
        
        public string Code5 { get; set; }
        
        public string Code6 { get; set; }

        public string ReturnUrl { get; set; }
    }
}