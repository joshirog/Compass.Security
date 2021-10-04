using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Compass.Security.Web.Commons.Extensions
{
    public class ProxyHttpsExtension
    {
        private const string ForwardedProtoHeader = "X-Forwarded-Proto";
        private readonly RequestDelegate _next;

        public ProxyHttpsExtension(RequestDelegate next) {
            _next = next;
        }

        public async Task Invoke(HttpContext ctx) {
            var h = ctx.Request.Headers;
            if (h[ForwardedProtoHeader] == string.Empty || h[ForwardedProtoHeader] == "https") {
                await _next(ctx);
            } else if (h[ForwardedProtoHeader] != "https") {
                var withHttps = $"https://{ctx.Request.Host}{ctx.Request.Path}{ctx.Request.QueryString}";
                ctx.Response.Redirect(withHttps);
            }
        }
    }
    
    public static class ReverseProxyEnforcerExtension {
        public static IApplicationBuilder UseReverseProxyEnforcerExtension(this IApplicationBuilder builder) {
            return builder.UseMiddleware<ProxyHttpsExtension>();
        }
    }
}