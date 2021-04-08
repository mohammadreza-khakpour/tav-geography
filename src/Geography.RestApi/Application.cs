using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Geography.Infrastructure;

namespace Geography.RestApi
{
    class Application
    {
        private static Configuration[] _configurations;

        static void Main(string[] args)
        {
            var baseDirectory = Directory.GetCurrentDirectory();
            var appSettings = ReadAppSettings(baseDirectory, "appsettings.json");
            var environment = appSettings.GetValue("environment", defaultValue: "Development");
            _configurations = GetConfiguratorsFromAssembly(typeof(Application).Assembly, args, appSettings, baseDirectory);
            InitializeConfigurators();
            var host = new WebHostBuilder()
                .UseContentRoot(baseDirectory)
                .UseEnvironment(environment)
                .ConfigureAppConfiguration(ConfigOptions)
                .ConfigureLogging(ConfigLogging)
                .UseStartup<Application>();
            ConfigServer(host);
            host.Build().Run();
        }

        public IServiceProvider ConfigureServices(IServiceCollection serviceContainer)
        {
            _configurations.ForEach(c => c.ConfigureServiceContainer(serviceContainer));

            var container = new ContainerBuilder();

            _configurations.ForEach(c => c.ConfigureServiceContainer(container));

            container.Populate(serviceContainer);

            return new AutofacServiceProvider(container.Build());
        }

        public void Configure(IApplicationBuilder appBuilder)
        {
            _configurations.ForEach(c => c.ConfigureApplication(appBuilder));
        }

        static IConfiguration ReadAppSettings(string baseDirectory, string filename)
        {
            return new ConfigurationBuilder()
                .SetBasePath(baseDirectory)
                .AddJsonFile(filename, optional: true, reloadOnChange: true)
                .Build();
        }

        static Configuration[] GetConfiguratorsFromAssembly(
            Assembly assembly,
            string[] args,
            IConfiguration appSettings,
            string baseDirectory)
        {
            void SetPropertyValue<T>(object obj, string name, object value)
            {
                typeof(T).GetProperty(name)?.SetValue(obj, value);
            }

            return assembly.GetTypes()
                .Where(_ => _.IsAbstract == false)
                .Where(typeof(Configuration).IsAssignableFrom)
                .Select(configuratorType => new
                {
                    Type = configuratorType,
                    Config = configuratorType.GetCustomAttribute<ConfigurationAttribute>()
                })
                .Where(_ => _.Config?.Disabled != true)
                .OrderBy(_ => _.Config?.Order ?? 0)
                .Select(_ =>
                {
                    var configurator = Activator.CreateInstance(_.Type) as Configuration;
                    SetPropertyValue<Configuration>(configurator, nameof(Configuration.CommandLineArgs), args);
                    SetPropertyValue<Configuration>(configurator, nameof(Configuration.BaseDirectory), baseDirectory);
                    SetPropertyValue<Configuration>(configurator, nameof(Configuration.AppSettings), appSettings);
                    return configurator;
                })
                .ToArray();
        }

        static void InitializeConfigurators()
        {
            _configurations.ForEach(_ => _.Initialized());
        }

        static void ConfigOptions(IConfigurationBuilder builder)
        {
            _configurations.ForEach(configurator => configurator.ConfigureSettings(builder));
        }

        static void ConfigLogging(WebHostBuilderContext context, ILoggingBuilder logging)
        {
            _configurations.ForEach(configurator => configurator.ConfigureLogging(context, logging));
        }

        static void ConfigServer(IWebHostBuilder hostBuilder)
        {
            _configurations.ForEach(_ => _.ConfigureServer(hostBuilder));
        }
    }
}
