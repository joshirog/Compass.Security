using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Compass.Security.Application.Commons.Constants;
using Compass.Security.Application.Commons.Interfaces;
using Newtonsoft.Json.Linq;

namespace Compass.Security.Infrastructure.Services.Externals.Google
{
    public class GoogleCaptchaService : ICaptchaService
    {
        public async Task<bool> SiteVerify(string captcha)
        {
            var endpoint = $"{ConfigurationConstant.GoogleCaptchaUrl}/siteverify?secret={ConfigurationConstant.GoogleCaptchaSecret}&response={captcha}";
            var request = (HttpWebRequest) WebRequest.Create(endpoint);
            using var response = request.GetResponse();
            using var stream = new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException());
            var jResponse = JObject.Parse(await stream.ReadToEndAsync());
            
            return jResponse.Value<bool>("success");
        }
    }
}