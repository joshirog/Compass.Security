using FluentValidation;

namespace Compass.Security.Application.Services.Accounts.Commands.Otp
{
    public class OtpValidator : AbstractValidator<OtpCommand>
    {
        public OtpValidator()
        {
            RuleFor(v => v.UserId)
                .NotEmpty();
        }
    }
}