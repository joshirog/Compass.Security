using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Compass.Security.Application.Commons.Behaviours
{
    public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<TRequest> _logger;

        public LoggingBehaviour(ILogger<TRequest> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var requestName = typeof(TRequest).Name;
            var responseName = typeof(TResponse).Name;
            
            try
            {
                _logger.LogInformation("Trading.Api Start: {Name} {@Request}", requestName, request);
                
                return await next();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Trading.Api: Unhandled Exception for Request {Name} {@Request}", requestName, request);
                throw;
            }
            finally
            {
                _logger.LogInformation("Trading.Api: {RequestName} {@Request} {ResposeName}", requestName, request, responseName);
            }
        }
    }
}