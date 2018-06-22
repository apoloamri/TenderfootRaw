using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using Tenderfoot.Tools.Extensions;

namespace Tenderfoot.TfSystem
{
    public static class TfSettings
    {
        private static IConfigurationRoot Configuration(string fileName = null, bool getDefault = false)
        {
            if (fileName.IsEmpty())
            {
                var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                environmentName = (environmentName.IsEmpty()) || getDefault ? string.Empty : $".{environmentName}";
                fileName = $"appsettings{environmentName}.json";
            }

            var cacheKey = $"tfconfiguration.{fileName}";
            var memoryCache = TfMemoryCache.Get<IConfigurationRoot>(cacheKey);
            if (memoryCache != null)
            {
                return memoryCache;
            }

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(fileName, optional: true, reloadOnChange: true)
                .Build();
            return TfMemoryCache.Set(cacheKey, configuration);
        }

        private static string GetSettingsBase(string fileName, string[] settings, bool getDefault = false)
        {
            var config = Configuration(fileName, getDefault);
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

        public static string GetSettings(params string[] settings)
        {
            var value = GetSettingsBase(null, settings, false);
            return value.IsEmpty() ? GetSettingsBase(null, settings, true) : value;
        }

        public static string GetSettingsWithFile(string fileName, params string[] settings)
        {
            var value = GetSettingsBase(fileName, settings, false);
            return value.IsEmpty() ? GetSettingsBase(fileName, settings, true) : value;
        }
        
        public static class Web
        {
            public static string[] AllowOrigins => GetSettings("Web", "AllowOrigins").Split(',');
            public static string ApiUrl => GetSettings("Web", "ApiUrl");
            public static int PaginateCount => Convert.ToInt32(GetSettings("Web", "PaginateCount"));
            public static bool RequireHttps => Convert.ToBoolean(GetSettings("Web", "RequireHttps"));
            public static int SessionTimeOut => Convert.ToInt32(GetSettings("Web", "SessionTimeOut"));
            public static string SiteUrl => GetSettings("Web", "SiteUrl");
            public static string SmtpEmail => GetSettings("Web", "SmtpEmail");
            public static string SmtpHost => GetSettings("Web", "SmtpHost");
            public static string SmtpPassword => GetSettings("Web", "SmtpPassword");
            public static int SmtpPort => Convert.ToInt32(GetSettings("Web", "SmtpPort"));
        }

        public static class Database
        {
            public static string Encoding => GetSettings("Database", "Encoding");
            public static string Server => GetSettings("Database", "Server");
            public static string Port => GetSettings("Database", "Port");
            public static string DatabaseName => GetSettings("Database", "Database");
            public static string UserId => GetSettings("Database", "UserId");
            public static string Password => GetSettings("Database", "Password");

            public static string ConnectionString
            {
                get
                {
                    string commandTimeout = GetSettings("Database", "CommandTimeout");
                    string timeout = GetSettings("Database", "Timeout");
                    string protocol = GetSettings("Database", "Protocol");
                    string ssl = GetSettings("Database", "SSL");
                    string sslMode = GetSettings("Database", "SslMode");
                    string pooling = GetSettings("Database", "Pooling");
                    string minPoolSize = GetSettings("Database", "MinPoolSize");
                    string maxPoolSize = GetSettings("Database", "MaxPoolSize");
                    string connectionLifeTime = GetSettings("Database", "ConnectionLifeTime");

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

            public static bool Migrate => Convert.ToBoolean(GetSettings("Database", "Migrate"));
            public static bool CreateDB => Convert.ToBoolean(GetSettings("Database", "CreateDB"));
        }

        public static class Encryption
        {
            public static bool Active => Convert.ToBoolean(GetSettings("Encryption", "Active"));
            public static string PasswordHash => GetSettings("Encryption", "PasswordHash");
            public static string SaltKey => GetSettings("Encryption", "SaltKey");
            public static string VIKey => GetSettings("Encryption", "VIKey");
        }

        public static class Logs
        {
            public static bool DBLogging => Convert.ToBoolean(GetSettings("Logs", "DBLogging"));
            public static string Migration => GetSettings("Logs", "Migration");
            public static string System => GetSettings("Logs", "System");
        }

        public static class System
        {
            private static IConfigurationSection ConfigurationSection = Configuration().GetSection("System");

            public static bool Debug => Convert.ToBoolean(GetSettings("System", "Debug"));
            public static string DefaultKey => GetSettings("System", "DefaultKey");
            public static string DefaultSecret => GetSettings("System", "DefaultSecret");
        }

        public static class SystemResources
        {
            private static IConfigurationSection ConfigurationSection = Configuration().GetSection("SystemResources");

            public static string EmailFiles => GetSettings("SystemResources", "EmailFiles");
            public static string FieldMessages => GetSettings("SystemResources", "FieldMessages");
            public static string SystemMessages => GetSettings("SystemResources", "SystemMessages");
        }
    }
}
