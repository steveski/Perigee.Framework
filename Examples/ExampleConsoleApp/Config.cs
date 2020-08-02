namespace ExampleConsoleApp
{
    public class Config
    {
        public DatabaseConfig Database { get; set; }
        public AesConfig Aes { get; set; }
    }

    public interface IAesConfig
    {
        string Key { get; set; }
        string Iv { get; set; }
    }

    public class AesConfig : IAesConfig
    {
        public string Key { get; set; }
        public string Iv { get; set; }

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
