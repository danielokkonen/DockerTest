using DockerTest.Persistence.Models;
using DockerTest.Test.TestFixtures;
using Microsoft.EntityFrameworkCore;

namespace DockerTest.Test.Persistance
{
    public class UserTests : DatabaseTestFixture
    {
        [Test]
        [TestCase(1)]
        public async Task Initial_User_Exists(int id)
        {
            // Act

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            // Assert

            Assert.That(_context.Users.Any(), Is.True);
            Assert.That(user, Is.Not.Null);
            Assert.That(user.Id, Is.EqualTo(id));
        }

        [Test]
        public void Manually_Assigned_Id_Throws_DbUpdateException()
        {
            // Arrange

            var user = new User(
                id: 1,
                name: "New User",
                email: "new.user@test.com");

            // Act

            _context.Users.Add(user);

            // Assert

            Assert.That(() => _context.SaveChangesAsync(), Throws.Exception.TypeOf<DbUpdateException>());
        }

        [Test]
        public void Too_Long_Name_Throws_DbUpdateException()
        {
            // Arrange

            var user = new User(
                id: 0,
                name: string.Join("a", Enumerable.Range(0, 500).Select(e => string.Empty)),
                email: "name.too.long@test.com");

            // Act

            _context.Users.Add(user);

            // Assert

            Assert.That(() => _context.SaveChangesAsync(), Throws.Exception.TypeOf<DbUpdateException>());
        }

        [Test]
        public void Too_Long_Email_Throws_DbUpdateException()
        {
            // Arrange

            var user = new User(
                id: 0,
                name: "Too Long Email",
                email: $"{string.Join("a", Enumerable.Range(0, 500).Select(e => string.Empty))}@test.com");

            // Act

            _context.Users.Add(user);

            // Assert

            Assert.That(() => _context.SaveChangesAsync(), Throws.Exception.TypeOf<DbUpdateException>());
        }
    }
}
