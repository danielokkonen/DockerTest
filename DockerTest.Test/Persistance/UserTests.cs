using DockerTest.Test.TestData;
using DockerTest.Test.TestFixtures;
using Microsoft.EntityFrameworkCore;

namespace DockerTest.Test.Persistance
{
    public class UserTests : DatabaseTestFixture
    {
        protected override void PrepareTestData()
        {
            _context.Users.AddRange(UserTestData.Create(10));
        }

        [Test]
        [TestCase(1)]
        public async Task Initial_User_Exists(int id)
        {
            // Act
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(_context.Users.Any(), Is.True);
                Assert.That(user, Is.Not.Null);
            });

            Assert.That(user.Id, Is.EqualTo(id));
        }

        [Test]
        public void Manually_Assigned_Id_Throws_DbUpdateException()
        {
            // Arrange
            var user = UserTestData.Create();
            user.UpdateId(1);

            // Act
            _context.Users.Add(user);

            // Assert
            Assert.That(() => _context.SaveChangesAsync(), Throws.Exception.TypeOf<DbUpdateException>());
        }

        [Test]
        public void Too_Long_Name_Throws_DbUpdateException()
        {
            // Arrange
            var invalidName = string.Join("a", Enumerable.Range(0, 500).Select(e => string.Empty));

            var user = UserTestData.Create();
            user.UpdateName(invalidName);

            // Act
            _context.Users.Add(user);

            // Assert
            Assert.That(() => _context.SaveChangesAsync(), Throws.Exception.TypeOf<DbUpdateException>());
        }

        [Test]
        public void Too_Long_Email_Throws_DbUpdateException()
        {
            // Arrange
            var invalidEmail = $"{string.Join("a", Enumerable.Range(0, 500).Select(e => string.Empty))}@test.com";

            var user = UserTestData.Create();
            user.UpdateEmail(invalidEmail);

            // Act
            _context.Users.Add(user);

            // Assert
            Assert.That(() => _context.SaveChangesAsync(), Throws.Exception.TypeOf<DbUpdateException>());
        }
    }
}
