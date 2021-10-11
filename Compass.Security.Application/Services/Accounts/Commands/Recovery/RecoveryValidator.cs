using FluentValidation;

namespace Compass.Security.Application.Services.Accounts.Commands.Recovery
{
    public class RecoveryValidator : AbstractValidator<RecoveryCommand>
    {
        public RecoveryValidator()
        {
            RuleFor(v => v.EmailOrPhone)
                .EmailAddress()
                .NotEmpty();
        }
    }
}