using FluentValidation;

namespace Compass.Security.Application.Services.Accounts.Commands.Resend
{
    public class ResendValidator : AbstractValidator<ResendCommand>
    {
        public ResendValidator()
        {
            RuleFor(v => v.UserId)
                .MaximumLength(36)
                .NotEmpty();
            
            RuleFor(v => v.Email)
                .MaximumLength(36)
                .NotEmpty();
        }
    }
}