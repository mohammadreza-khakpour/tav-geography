using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Geography.RestApi.Configs
{
    class VersioningConfig : Configuration
    {
        private ApiVersion _defaultVersion;
        private IApiVersionReader _versionReader;

        public override void Initialized()
        {
            _defaultVersion = ApiVersion.Parse("1.0");
            _versionReader = new MediaTypeApiVersionReader("version");
        }

        public override void ConfigureServiceContainer(IServiceCollection container)
        {
            container.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = _defaultVersion;
                options.ApiVersionReader = _versionReader;
            });

            container.AddVersionedApiExplorer(options =>
            {
                var vOptions = container.BuildServiceProvider().GetService<IOptions<ApiVersioningOptions>>()?.Value;

                if (vOptions?.DefaultApiVersion != null)
                {
                    options.DefaultApiVersion = vOptions.DefaultApiVersion;
                }

                options.GroupNameFormat = "'v'VVVV";
            });
        }
    }
}
