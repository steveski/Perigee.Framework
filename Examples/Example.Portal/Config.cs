namespace Example.Portal
{
    public class Config
    {
        public DatabaseConfig Database { get; set; }
    }

    public interface IDatabaseConfig
    {
        InMemoryConfig InMemory { get; set; }
        string ConnectionString { get; set; }
    }

    public class DatabaseConfig : IDatabaseConfig
    {
        public InMemoryConfig InMemory { get; set; }
        public string ConnectionString { get; set; }

    }

    public class InMemoryConfig
    {
        public bool Enabled { get; set; }
        public string Name { get; set; }

    }
}
