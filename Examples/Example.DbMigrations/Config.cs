namespace Example.DbMigrations
{
    public class Config
    {
        public DatabaseConfig Database { get; set; }

    }

    public interface IDatabaseConfig
    {
        string ConnectionString { get; set; }
    }

    public class DatabaseConfig : IDatabaseConfig
    {
        public string ConnectionString { get; set; }

    }
   
}
