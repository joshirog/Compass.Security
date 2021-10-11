using System.Threading;
using System.Threading.Tasks;
using Compass.Security.Application.Commons.Interfaces;
using MediatR;

namespace Compass.Security.Application.Services.Accounts.Commands.SignOut
{
    public class SignOutHandler : IRequestHandler<SignOutCommand>
    {
        private readonly IIdentityService _identityService;

        public SignOutHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        
        public async Task<Unit> Handle(SignOutCommand request, CancellationToken cancellationToken)
        {
            await _identityService.SignOut();
            
            return await Task.FromResult(Unit.Value);
        }
    }
}