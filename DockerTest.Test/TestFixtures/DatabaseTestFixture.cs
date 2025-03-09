using DockerTest.Persistence;
using DockerTest.Test.Containers;
using Microsoft.EntityFrameworkCore;
using Respawn;
using Respawn.Graph;

namespace DockerTest.Test.TestFixtures
{
    [TestFixture]
    public abstract class DatabaseTestFixture
    {
        protected DatabaseContext _context;
        protected Respawner _respawner;

        protected abstract void PrepareTestData();

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            var connectionString = DatabaseContainer.Instance.GetConnectionString();

            _context = CreateDatabaseContext(connectionString);
            await _context.Database.MigrateAsync();

            _respawner = await Respawner.CreateAsync(connectionString, new RespawnerOptions
            {
                TablesToIgnore = [new Table("dbo", "__EFMigrationsHistory")],
                DbAdapter = DbAdapter.SqlServer,
            });
        }

        [SetUp]
        public async Task SetUp()
        {
            var connectionString = DatabaseContainer.Instance.GetConnectionString();

            _context = CreateDatabaseContext(connectionString);

            PrepareTestData();
            await _context.SaveChangesAsync();
        }

        [TearDown]
        public async Task TearDown()
        {
            var connectionString = DatabaseContainer.Instance.GetConnectionString();

            await _respawner.ResetAsync(connectionString);
            await _context.DisposeAsync();
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            await _context.DisposeAsync();
        }

        private static DatabaseContext CreateDatabaseContext(string connectionString)
        {
            var dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>()
                .UseSqlServer(connectionString)
                .Options;

            return new DatabaseContext(dbContextOptions);
        }
    }
}
