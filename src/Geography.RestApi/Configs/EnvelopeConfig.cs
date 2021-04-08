using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Geography.RestApi.Configs
{
    class EnvelopeConfig : Configuration
    {
        public override void ConfigureApplication(IApplicationBuilder app)
        {
            var environment = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();
            var jsonOptions = app.ApplicationServices.GetService<IOptions<JsonOptions>>()?.Value.JsonSerializerOptions;

            app.UseExceptionHandler(_ => _.Run(async context =>
            {
                var exception = context.Features.Get<IExceptionHandlerPathFeature>()?.Error;
                var errorType = exception?.GetType().Name;
                var errorDescription = environment.IsProduction() ? null : exception?.ToString();
                var result = new
                {
                    Error = errorType,
                    Description = errorDescription
                };

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = MediaTypeNames.Application.Json;
                await context.Response.WriteAsync(JsonSerializer.Serialize(result, jsonOptions));
            }));

            if (environment.IsProduction())
            {
                app.UseHsts();
            }
        }
    }
}
