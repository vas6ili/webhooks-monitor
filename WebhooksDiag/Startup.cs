using App.Metrics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace WebhooksDiag
{
    public class Startup
    {
        public Startup()
        {
            Metrics = new MetricsBuilder()
                .OutputMetrics.AsJson()
                .OutputMetrics.AsPlainText()
                .Build();
        }

        public IMetricsRoot Metrics { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMetrics(Metrics);
            services.AddMetricsEndpoints();
            services.AddSingleton<WebhookMetrics>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Map("/accept", mapped => mapped.UseMiddleware<WebhooksMiddleware>());

            app.UseMetricsAllEndpoints();

            app.UseStaticFiles();
        }
    }
}
