using DotNet.Testcontainers.Builders;
using Testcontainers.MsSql;

namespace DockerTest.Test.Containers
{
    internal static class DatabaseContainer
    {
        internal static MsSqlContainer Instance => _msSql.Value;

        private static readonly Lazy<MsSqlContainer> _msSql = new(StartContainer);

        private static MsSqlContainer StartContainer()
        {
            var container = new MsSqlBuilder()
                .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
                .WithEnvironment("ACCEPT_EULA", "Y")
                .WithExposedPort(1433)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(1433))
                .WithCleanUp(true)
                .Build();

            container.StartAsync().GetAwaiter().GetResult();

            return container;
        }
    }
}
