using System;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;

namespace Geography.RestApi
{
    public abstract class Configuration
    {
        public string BaseDirectory { get; private set; }
        public IConfiguration AppSettings { get; private set; }
        public string[] CommandLineArgs { get; private set; }

        public virtual void Initialized()
        {
        }

        public virtual void ConfigureServiceContainer(IServiceCollection container)
        {
        }
        public virtual void ConfigureServiceContainer(ContainerBuilder container)
        {
        }

        public virtual void ConfigureApplication(IApplicationBuilder app)
        {
        }

        public virtual void ConfigureLogging(WebHostBuilderContext context, ILoggingBuilder logging)
        {
        }

        public virtual void ConfigureServer(IWebHostBuilder host)
        {
        }

        public virtual void ConfigureSettings(IConfigurationBuilder config)
        {
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class ConfigurationAttribute : Attribute
    {
        public bool Disabled { get; set; }
        public int Order { get; set; }
    }
}
