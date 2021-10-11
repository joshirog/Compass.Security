using FluentValidation;

namespace Compass.Security.Application.Services.Accounts.Commands.Confirm
{
    public class ConfirmValidator : AbstractValidator<ConfirmCommand>
    {
        public ConfirmValidator()
        {
            RuleFor(v => v.UserId)
                .MaximumLength(36)
                .NotEmpty();
        }
    }
}