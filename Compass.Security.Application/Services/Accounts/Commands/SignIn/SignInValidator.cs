using FluentValidation;

namespace Compass.Security.Application.Services.Accounts.Commands.SignIn
{
    public class SignInValidator : AbstractValidator<SignInCommand>
    {
        public SignInValidator()
        {
            RuleFor(v => v.Username)
                .MaximumLength(200)
                .NotEmpty()
                .EmailAddress();
            
            RuleFor(v => v.Password)
                .MaximumLength(200)
                .NotEmpty();
        }
    }
}