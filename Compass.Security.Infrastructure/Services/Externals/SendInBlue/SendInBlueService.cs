using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Compass.Security.Application.Commons.Dtos;
using Compass.Security.Application.Commons.Interfaces;
using Compass.Security.Infrastructure.Commons.Constants;
using Microsoft.Extensions.Logging;

namespace Compass.Security.Infrastructure.Services.Externals.SendInBlue
{
    public class SendInBlueService : INotificationService
    {
        private readonly ILogger<SendInBlueService> _logger;
        private readonly IDateTimeService _dateTimeService;
        private readonly IHttpClientFactory _clientFactory;

        public SendInBlueService(ILogger<SendInBlueService> logger, IDateTimeService dateTimeService, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _dateTimeService = dateTimeService;
            _clientFactory = clientFactory;
        }

        public async Task SendEmail(EmailDto entity)
        {
            _logger.LogInformation("[SendInBlueClient - {@Datetime}] : Sending email ... to {@Subject}", _dateTimeService.Now, entity.Subject);
            
            var client = _clientFactory.CreateClient(EndpointConstant.SendInBlue);
            
            var response = await client.PostAsync(EndpointConstant.SendInBlueUriEmail, 
                new StringContent(JsonSerializer.Serialize(entity, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                }), Encoding.UTF8, MediaTypeNames.Application.Json));
            
            var result = await response.Content.ReadAsStringAsync();
            
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("[SendInBlueClient - {@Datetime}] : {@Id}", _dateTimeService.Now, result);
            }
            else
            {
                _logger.LogError("[SendInBlueClient - {@Datetime}] : error {@Id}", _dateTimeService.Now, result);
            }
        }
    }
}