using System;
using System.Net;
using Compass.Security.Application.Commons.Constants;
using Compass.Security.Application.Commons.Interfaces;
using LazyCache;

namespace Compass.Security.Infrastructure.Services.Internals.LazyCache
{
    public class LazyCacheService : ICacheService
    {
        private readonly IAppCache _cache;
        
        public LazyCacheService(IAppCache cache)
        {
            _cache = cache;
        }

        public string Template(string key)
        {
            var result = _cache.GetOrAdd(key, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromDays(1);
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1);
                
                var web = new WebClient();

                return key switch
                {
                    nameof(ConfigurationConstant.TemplateActivation) => web.DownloadString(ConfigurationConstant.TemplateActivation),
                    nameof(ConfigurationConstant.TemplateWelcome) => web.DownloadString(ConfigurationConstant.TemplateWelcome),
                    nameof(ConfigurationConstant.TemplatePassword) => web.DownloadString(ConfigurationConstant.TemplatePassword),
                    nameof(ConfigurationConstant.TemplateOtp) => web.DownloadString(ConfigurationConstant.TemplateOtp),
                    nameof(ConfigurationConstant.TemplateReset) => web.DownloadString(ConfigurationConstant.TemplateReset),
                    _ => null
                };
            });

            return result;
        }
        
        public void Remove(string key)
        {
            _cache.Remove(key);
        }
    }
}