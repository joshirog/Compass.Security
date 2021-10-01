using Compass.Security.Application.Commons.Dtos;
using MediatR;

namespace Compass.Security.Application.Services.Accounts.Commands.SignIn
{
    public class SignInCommand : IRequest<ResponseDto<SignInResponse>>
    {
        
    }
}