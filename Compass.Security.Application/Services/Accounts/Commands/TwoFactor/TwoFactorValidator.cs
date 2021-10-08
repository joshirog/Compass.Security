using System.Collections.Generic;
using System.Linq;
using FluentValidation;

namespace Compass.Security.Application.Services.Accounts.Commands.TwoFactor
{
    public class TwoFactorValidator: AbstractValidator<TwoFactorCommand>
    {
        public TwoFactorValidator()
        {
            RuleFor(v => v.Id)
                .MaximumLength(36)
                .NotEmpty();
            
            RuleFor(d => d).Custom((d, c) =>
            {
                var errors = CustomValidator(d.Code1, d.Code2, d.Code3, d.Code4, d.Code5, d.Code6);

                foreach (var error in errors.Distinct()) 
                    c.AddFailure(error.Split("|")[0], error.Split("|")[1]);
            });
        }
        
        private static IEnumerable<string> CustomValidator(string code1, string code2, string code3, string code4, string code5, string code6)
        {
            var errors = new List<string>();
            
            var codeEmpty1 = string.IsNullOrEmpty(code1);
            var codeEmpty2 = string.IsNullOrEmpty(code2);
            var codeEmpty3 = string.IsNullOrEmpty(code3);
            var codeEmpty4 = string.IsNullOrEmpty(code4);
            var codeEmpty5 = string.IsNullOrEmpty(code5);
            var codeEmpty6 = string.IsNullOrEmpty(code6);

            if (codeEmpty1 || codeEmpty2 || codeEmpty3 || codeEmpty4 || codeEmpty5 || codeEmpty6)
            {
                errors.Add($"|The code cannot be empty");
                return errors;
            }
            
            var codeInt1 = int.TryParse(code1, out _);
            var codeInt2 = int.TryParse(code2, out _);
            var codeInt3 = int.TryParse(code3, out _);
            var codeInt4 = int.TryParse(code4, out _);
            var codeInt5 = int.TryParse(code5, out _);
            var codeInt6 = int.TryParse(code6, out _);

            if (!codeInt1 || !codeInt2 || !codeInt3 || !codeInt4 || !codeInt5 || !codeInt6)
            {
                errors.Add($"|The code must be numeric");
                return errors;
            }
            
            var codeLength1 = code1.Length.Equals(1);
            var codeLength2 = code2.Length.Equals(1);
            var codeLength3 = code3.Length.Equals(1);
            var codeLength4 = code4.Length.Equals(1);
            var codeLength5 = code5.Length.Equals(1);
            var codeLength6 = code6.Length.Equals(1);

            if (codeLength1 && codeLength2 && codeLength3 && codeLength4 && codeLength5 && codeLength6) 
                return errors;
            
            errors.Add($"|You only have to enter one number per input");
            
            return errors;
        }
    }
}