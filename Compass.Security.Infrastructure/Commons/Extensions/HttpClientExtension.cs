using System;
using System.Net.Http.Headers;
using System.Net.Mime;
using Compass.Security.Application.Commons.Constants;
using Compass.Security.Infrastructure.Commons.Constants;
using Microsoft.Extensions.DependencyInjection;

namespace Compass.Security.Infrastructure.Commons.Extensions
{
    public static class HttpClientExtension
    {
        public static void AddHttpClientExtension(this IServiceCollection services)
        {
            services.AddHttpClient(SettingConstant.SendInBlue, c =>
            {
                c.BaseAddress = new Uri(EndpointConstant.SendInBlueEndpoint);
                c.DefaultRequestHeaders.Connection.Add(SettingConstant.KeepAlive);
                c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
                c.DefaultRequestHeaders.Add(SettingConstant.ApiKey, ConfigurationConstant.SendInBlueApiKey);
            });
        }
    }
}