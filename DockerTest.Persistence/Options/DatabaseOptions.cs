namespace DockerTest.Persistence.Options
{
    public class DatabaseOptions
    {
        public const string Section = "Database";

        public required string Server { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string ConnectionString { get; set; }
    }
}
