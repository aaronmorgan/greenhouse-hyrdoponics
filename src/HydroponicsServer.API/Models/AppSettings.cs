using Microsoft.Extensions.Configuration;

namespace HydroponicsServer.Models
{
    public class AppSettings
    {
        [ConfigurationKeyName(name: "ASPNETCORE_ENVIRONMENT")]
        public string Environment { get; set; }

        [ConfigurationKeyName(name: "POSTGRESQL_CONNECTION_STRING")]
        public string PostgresqlConnectionString { get; set; }
    }
}
