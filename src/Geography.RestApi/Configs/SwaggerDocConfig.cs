using System;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Text.Json;
using Geography.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Geography.RestApi.Configs
{
    class SwaggerDocConfig : Configuration
    {
        private const string ModuleTitle = "Geography";

        public override void ConfigureServiceContainer(IServiceCollection container)
        {
            container.AddRouting();

            container.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(_ => _.FullName);

                var versionProvider = container.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
                versionProvider.ApiVersionDescriptions.ForEach(description =>
                {
                    options.SwaggerDoc(description.GroupName, CreateApiInfo(description));
                });

                options.DescribeAllEnumsAsStrings();

                //options.OperationFilter<SwaggerDefaultValues>();

                var xmlDocFilepath = GetXmlDocumentationFilePath(typeof(Application).Assembly);
                if (File.Exists(xmlDocFilepath))
                    options.IncludeXmlComments(xmlDocFilepath);
            });
        }

        public override void ConfigureApplication(IApplicationBuilder app)
        {
            var provider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/docs/versions", async context =>
                {
                    var versions = provider.ApiVersionDescriptions.Select(_ => new
                    {
                        Name = _.GroupName,
                        IsDeprecated = _.IsDeprecated,
                        Version = _.ApiVersion.ToString()
                    });

                    var jsonSettings = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    };
                    var jsonContent = JsonSerializer.Serialize(versions, jsonSettings);
                    context.Response.ContentType = MediaTypeNames.Application.Json;
                    context.Response.StatusCode = StatusCodes.Status200OK;
                    await context.Response.WriteAsync(jsonContent);
                });
            });

            app.UseSwagger(options =>
            {
                options.RouteTemplate = "docs/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = "docs";

                provider.ApiVersionDescriptions.ForEach(_ =>
                    options.SwaggerEndpoint($"{_.GroupName}/swagger.json", _.GroupName));

                options.DocumentTitle = $"{ModuleTitle} API Documentation";
            });
        }

        OpenApiInfo CreateApiInfo(ApiVersionDescription description)
        {
            var info = new OpenApiInfo
            {
                Title = $"{ModuleTitle} API {description.ApiVersion}",
                Version = description.ApiVersion.ToString(),
                Description = "API Documentation.",
                Contact = new OpenApiContact
                {
                    Name = "TaavSystem",
                    Url = new Uri("https://taavsys.ir"),
                    Email = "admin@taavsys.ir"
                },
                TermsOfService = new Uri("https://taavsys.ir/terms"),
                License = new OpenApiLicense
                {
                    Name = "TaavSystem",
                    Url = new Uri("https://taavsys.ir/license")
                }
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }

        string GetXmlDocumentationFilePath(Assembly assembly)
        {
            var basePath = Path.GetDirectoryName(assembly.Location);
            var fileName = $"{assembly.GetName().Name}.xml";
            return Path.Combine(basePath, fileName);
        }
    }

    class SwaggerDefaultValues : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                return;
            }

            foreach (var parameter in operation.Parameters)
            {
                var description = context.ApiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);
                var routeInfo = description.RouteInfo;

                if (parameter.Description == null)
                {
                    parameter.Description = description.ModelMetadata?.Description;
                }

                if (routeInfo == null)
                {
                    continue;
                }

                parameter.Required |= !routeInfo.IsOptional;
            }
        }
    }
}
