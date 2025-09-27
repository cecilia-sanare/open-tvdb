using OpenTVDB.API.Enums;

namespace OpenTVDB.API;

public class Config
{
    public DatabaseConfig Database { get; set; }

    public class DatabaseConfig
    {
        public DatabaseType Type { get; set; }
        public string ConnectionString { get; set; } = Guid.NewGuid().ToString();
    }
}
