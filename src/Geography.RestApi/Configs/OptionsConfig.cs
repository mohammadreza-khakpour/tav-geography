using Microsoft.Extensions.Configuration;

namespace Geography.RestApi.Configs
{
    class OptionsConfig : Configuration
    {
        public override void ConfigureSettings(IConfigurationBuilder config)
        {
            config.Sources.Clear();
            config.SetBasePath(BaseDirectory)
                  .AddConfiguration(AppSettings)
                  .AddCommandLine(CommandLineArgs)
                  .AddEnvironmentVariables();
        }
    }
}
