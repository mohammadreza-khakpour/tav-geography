using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Geography.RestApi.Configs
{
    class HealthCheckConfig : Configuration
    {
        private const string Url = "/health";

        public override void ConfigureServiceContainer(IServiceCollection container)
        {
            container.AddRouting();
            container.AddHealthChecks();
        }

        public override void ConfigureApplication(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapHealthChecks(Url));
        }
    }
}
