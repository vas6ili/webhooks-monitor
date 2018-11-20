using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace WebhooksMonitor
{
    public class WebhooksMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly WebhookMetrics _metrics;
        private readonly JsonSerializer _serializer;

        public WebhooksMiddleware(RequestDelegate next, WebhookMetrics metrics)
        {
            _next = next;
            _metrics = metrics;
            _serializer = JsonSerializer.CreateDefault();
        }

        public Task InvokeAsync(HttpContext ctx)
        {
            if (!string.Equals(ctx.Request.Method, "POST", StringComparison.Ordinal))
            {
                ctx.Response.StatusCode = 405;
                return Task.CompletedTask;
            }

            var reader = new JsonTextReader(new StreamReader(ctx.Request.Body));
            var e = _serializer.Deserialize<WebhookEvent>(reader);


            _metrics.Mark(e);


            ctx.Response.StatusCode = 202;
            return Task.CompletedTask;
        }
    }
}
