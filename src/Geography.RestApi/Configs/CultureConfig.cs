using System.Globalization;
using Microsoft.AspNetCore.Builder;

namespace Geography.RestApi.Configs
{
    class CultureConfig : Configuration
    {
        public override void ConfigureApplication(IApplicationBuilder app)
        {
            var culture = CultureInfo.GetCultureInfo("en-US");
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
        }
    }
}
