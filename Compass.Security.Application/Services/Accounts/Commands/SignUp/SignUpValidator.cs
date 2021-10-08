using System;
using System.Threading.Tasks;
using Compass.Security.Application.Commons.Interfaces;
using Compass.Security.Domain.Enums;
using FluentValidation;

namespace Compass.Security.Application.Services.Accounts.Commands.SignUp
{
    public class SignUpValidator : AbstractValidator<SignUpCommand>
    {
        private readonly IBlacklistRepository _blacklistRepository;

        public SignUpValidator(IBlacklistRepository blacklistRepository)
        {
            _blacklistRepository = blacklistRepository;

            RuleFor(v => v.Firstname)
                .MaximumLength(100)
                .NotEmpty();
            
            RuleFor(v => v.Lastname)
                .MaximumLength(100)
                .NotEmpty();
            
            RuleFor(v => v.Email)
                .MaximumLength(200)
                .NotEmpty()
                .EmailAddress();

            RuleFor(v => v.Password)
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(100)
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W]).{8,}$").WithMessage("The password is insecure, please try entering a new one. It must contain at least one lowercase letter, one uppercase letter, numbers, and at least one special character.")
                //.Matches("^[A-Z]+$").WithMessage("Password requires at least one uppercase letter")
                //.Matches(@"[a-z]+").WithMessage("The password requires at least one lowercase letter.")
                //.Matches(@"[0-9]+").WithMessage("The password requires at least one digit.")
                //.Matches(@"[\!\?\*\.]+").WithMessage("Your password must contain at least one (!? *.).")
                .MustAsync((password, _) => CheckBackList(password)).WithMessage("The password is insecure, please try entering a new one.");
        }
        
        private async Task<bool> CheckBackList(string password)
        {
            var result = await _blacklistRepository.AnyAsync(x =>
                x.Type.Equals(Enum.GetName(typeof(BlackListTypeEnum), BlackListTypeEnum.Password)) &&
                x.Value.Equals(password));
            
            return !result;
        }
    }
}