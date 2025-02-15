using Bogus;
using DockerTest.Persistence.Models;

namespace DockerTest.Test.TestData
{
    internal static class UserTestData
    {
        internal static List<User> Create(int count)
        {
            var faker = new Faker<User>()
                .RuleFor(u => u.Name, (f, u) => f.Name.FullName())
                .RuleFor(u => u.Email, (f, u) => $"{u.Name.Replace(" ", ".")}@test.com");

            return faker.Generate(count);
        }
    }
}
