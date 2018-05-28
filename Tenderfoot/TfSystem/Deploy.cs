using Microsoft.Extensions.Configuration;
using System.IO;

namespace Tenderfoot.TfSystem
{
    public static class Deploy
    {
        public static IConfigurationRoot Configuration()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("deploy.json", optional: true, reloadOnChange: true);

            return configurationBuilder.Build();
        }

        public static string Deployment => Configuration().GetSection("Deployment").Value;
    }
}
