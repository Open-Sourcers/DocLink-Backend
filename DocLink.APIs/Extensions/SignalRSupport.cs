using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace DocLink.APIs.Extensions
{
    public static class SignalRSupport
    {
        public static JwtBearerOptions AddSignalRSupport(this JwtBearerOptions options, string hubPath)
        {
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    // استخراج access_token من الـ Query String
                    var accessToken = context.Request.Query["access_token"];
                    var path = context.HttpContext.Request.Path;

                    // إذا كان الـ token موجود والمسار يبدأ بالـ hubPath المحدد
                    if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments(hubPath))
                    {
                        context.Token = accessToken;
                    }
                    return Task.CompletedTask;
                }
            };

            return options;
        }
    }
}
