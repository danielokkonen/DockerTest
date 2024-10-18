using DockerTest.Persistence;
using DockerTest.Persistence.Models;
using DotNet.Testcontainers.Builders;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;

namespace DockerTest.Test
{
    public class UserTests
    {
        private readonly MsSqlContainer _msSql = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithEnvironment("ACCEPT_EULA", "Y")
            .WithExposedPort(1433)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(1433))
            .WithCleanUp(true)
            .Build();

        private DatabaseContext _dbContext;

        [OneTimeSetUp]
        public async Task Setup()
        {
            await _msSql.StartAsync();

            var connectionString = _msSql.GetConnectionString();

            var dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>()
                .UseSqlServer(connectionString)
                .Options;

            _dbContext = new DatabaseContext(dbContextOptions);

            await _dbContext.Database.MigrateAsync();

            _dbContext.Users.Add(new User
            {
                Name = "Test Testsson",
                Email = "test.testsson@test.com"
            });

            await _dbContext.SaveChangesAsync();
        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
            _dbContext.Dispose();
            await _msSql.DisposeAsync();
        }

        [Test]
        public void Initial_User_Exists()
        {
            var user = _dbContext.Users.FirstOrDefault();

            Assert.That(user, Is.Not.Null);
        }

        [Test]
        public void User_With_Same_Id_Cannot_Be_Added()
        {
            Assert.Throws<InvalidOperationException>(() =>
                _dbContext.Users.Add(new User
                {
                    Id = 1,
                    Name = "Invalid User",
                    Email = "invalid.user@test.com",
                }));
        }

        [Test]
        public void User_With_Too_Long_Name_Cannot_Be_Inserted()
        {
            var user = new User
            {
                Name = string.Join("a", Enumerable.Range(0, 500).Select(e => string.Empty)),
                Email = "too.long@test.com",
            };

            _dbContext.Users.Add(user);
            Assert.ThrowsAsync<DbUpdateException>(async () => await _dbContext.SaveChangesAsync());
        }
    }
}