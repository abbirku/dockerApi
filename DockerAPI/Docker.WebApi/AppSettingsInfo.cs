using Microsoft.Extensions.Configuration;
using System.IO;

namespace Docker.WebApi
{
    public static class AppSettingsInfo
    {
        // Get a valued stored in the appsettings.
        // Pass in a key like TestArea:TestKey to get Value
        public static T GetCurrentValue<T>(string Key)
        {
            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                            .AddEnvironmentVariables();

            IConfigurationRoot configuration = builder.Build();

            return configuration.GetValue<T>(Key);
        }
    }
}
