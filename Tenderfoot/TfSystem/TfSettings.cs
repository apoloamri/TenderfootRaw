using Tenderfoot.Database;
using Tenderfoot.Tools.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;

namespace Tenderfoot.TfSystem
{
    public static class TfSettings
    {
        public static IConfigurationRoot Configuration(string fileName = null)
        {
            if (fileName.IsEmpty())
            {
                string deployment = Deploy.Deployment;
                string deploymentString = deployment.IsEmpty() ? "" : $".{deployment}";
                fileName = $"appsettings{deploymentString}.json";
            }

            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(fileName, optional: true, reloadOnChange: true);

            return configurationBuilder.Build();
        }

        public static object GetSettings(params string[] settings)
        {
            var config = Configuration();
            var section = config.GetSection(settings.FirstOrDefault());

            var skipFirst = true;
            foreach (var setting in settings)
            {
                if (skipFirst)
                {
                    skipFirst = false;
                    continue;
                }
                section = section.GetSection(setting);
            }

            return section.Value;
        }

        public static class Web
        {
            private static IConfigurationSection ConfigurationSection = Configuration().GetSection("Web");

            public static string[] AllowOrigins => ConfigurationSection.GetSection("AllowOrigins").Value.Split(',');
            public static string ApiUrl => ConfigurationSection.GetSection("ApiUrl").Value;
            public static int SessionTimeOut => Convert.ToInt32(ConfigurationSection.GetSection("SessionTimeOut").Value);
            public static bool RequireHttps => Convert.ToBoolean(ConfigurationSection.GetSection("RequireHttps").Value);
            public static string SiteUrl => ConfigurationSection.GetSection("SiteUrl").Value;
            public static string SmtpEmail => ConfigurationSection.GetSection("SmtpEmail").Value;
            public static string SmtpHost => ConfigurationSection.GetSection("SmtpHost").Value;
            public static string SmtpPassword => ConfigurationSection.GetSection("SmtpPassword").Value;
            public static int SmtpPort => Convert.ToInt32(ConfigurationSection.GetSection("SmtpPort").Value);
        }

        public static class Database
        {
            private static IConfigurationSection ConfigurationSection = Configuration().GetSection("Database");

            public static string Encoding => ConfigurationSection.GetSection("Encoding").Value;
            public static string Server => ConfigurationSection.GetSection("Server").Value;
            public static string Port => ConfigurationSection.GetSection("Port").Value;
            public static string DatabaseName => ConfigurationSection.GetSection("Database").Value;
            public static string UserId => ConfigurationSection.GetSection("UserId").Value;
            public static string Password => ConfigurationSection.GetSection("Password").Value;

            public static string ConnectionString
            {
                get
                {
                    string commandTimeout = ConfigurationSection.GetSection("CommandTimeout").Value;
                    string timeout = ConfigurationSection.GetSection("Timeout").Value;
                    string protocol = ConfigurationSection.GetSection("Protocol").Value;
                    string ssl = ConfigurationSection.GetSection("SSL").Value;
                    string sslMode = ConfigurationSection.GetSection("SslMode").Value;
                    string pooling = ConfigurationSection.GetSection("Pooling").Value;
                    string minPoolSize = ConfigurationSection.GetSection("MinPoolSize").Value;
                    string maxPoolSize = ConfigurationSection.GetSection("MaxPoolSize").Value;
                    string connectionLifeTime = ConfigurationSection.GetSection("ConnectionLifeTime").Value;

                    return
                        (!Server.IsEmpty() ? $"Server={Server};" : string.Empty) +
                        (!Port.IsEmpty() ? $"Port={Port};" : string.Empty) +
                        (!DatabaseName.IsEmpty() ? $"Database={DatabaseName};" : string.Empty) +
                        (!UserId.IsEmpty() ? $"User Id={UserId};" : string.Empty) +
                        (!Password.IsEmpty() ? $"Password={Password};" : string.Empty) +
                        (!commandTimeout.IsEmpty() ? $"CommandTimeout={commandTimeout};" : string.Empty) +
                        (!timeout.IsEmpty() ? $"Timeout={timeout};" : string.Empty) +
                        (!protocol.IsEmpty() ? $"Protocol={protocol};" : string.Empty) +
                        (!ssl.IsEmpty() ? $"SSL={ssl};" : string.Empty) +
                        (!sslMode.IsEmpty() ? $"SslMode={sslMode};" : string.Empty) +
                        (!pooling.IsEmpty() ? $"Pooling={pooling};" : string.Empty) +
                        (!minPoolSize.IsEmpty() ? $"MinPoolSize={minPoolSize};" : string.Empty) +
                        (!maxPoolSize.IsEmpty() ? $"MaxPoolSize={maxPoolSize};" : string.Empty) +
                        (!connectionLifeTime.IsEmpty() ? $"ConnectionLifeTime={connectionLifeTime};" : string.Empty);
                }
            }

            public static string DefaultConnectionString
            {
                get
                {
                    return
                        (!Server.IsEmpty() ? $"Server={Server};" : string.Empty) +
                        (!Port.IsEmpty() ? $"Port={Port};" : string.Empty) +
                        $"Database=postgres;" +
                        (!UserId.IsEmpty() ? $"User Id={UserId};" : string.Empty) +
                        (!Password.IsEmpty() ? $"Password={Password};" : string.Empty);
                }
            }

            public static bool Migrate => Convert.ToBoolean(ConfigurationSection.GetSection("Migrate").Value);
        }

        public static class Encryption
        {
            private static IConfigurationSection ConfigurationSection = Configuration().GetSection("Encryption");

            public static bool Active => Convert.ToBoolean(ConfigurationSection.GetSection("Active").Value);
            public static string PasswordHash => ConfigurationSection.GetSection("PasswordHash").Value;
            public static string SaltKey => ConfigurationSection.GetSection("SaltKey").Value;
            public static string VIKey => ConfigurationSection.GetSection("VIKey").Value;
        }

        public static class Logs
        {
            private static IConfigurationSection ConfigurationSection = Configuration().GetSection("Logs");

            public static bool DBLogging => Convert.ToBoolean(ConfigurationSection.GetSection("DBLogging").Value);
            public static string Migration => ConfigurationSection.GetSection("Migration").Value;
            public static string System => ConfigurationSection.GetSection("System").Value;
        }

        public static class System
        {
            private static IConfigurationSection ConfigurationSection = Configuration().GetSection("System");

            public static bool Debug => Convert.ToBoolean(ConfigurationSection.GetSection("Debug").Value);
            public static string DefaultKey => ConfigurationSection.GetSection("DefaultKey").Value;
            public static string DefaultSecret => ConfigurationSection.GetSection("DefaultSecret").Value;
        }

        public static class SystemResources
        {
            private static IConfigurationSection ConfigurationSection = Configuration().GetSection("SystemResources");

            public static string EmailFiles => ConfigurationSection.GetSection("EmailFiles").Value;
            public static string FieldMessages => ConfigurationSection.GetSection("FieldMessages").Value;
            public static string SystemMessages => ConfigurationSection.GetSection("SystemMessages").Value;
        }
    }
}
