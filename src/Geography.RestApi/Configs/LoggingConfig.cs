using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Geography.RestApi.Configs
{
    class LoggingConfig : Configuration
    {
        private const string LevelConfigKey = "logging:level";

        public override void ConfigureLogging(WebHostBuilderContext context, ILoggingBuilder logging)
        {
            logging.ClearProviders();
            if (context.HostingEnvironment.IsDevelopment())
            {
                logging.SetMinimumLevel(LogLevel.Debug)
                       .AddConsole()
                       .AddDebug()
                       .AddEventSourceLogger();
            }
            else
            {
                logging.SetMinimumLevel(context.Configuration.GetValue<LogLevel>(LevelConfigKey, LogLevel.Warning));
            }
        }
    }
}
