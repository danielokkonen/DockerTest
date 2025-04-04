namespace DockerTest.Persistence.Models
{
    public class User
    {
        public User() { }

        public User(int id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }

        public int Id { get; private set; }
        public string? Name { get; private set; }
        public string? Email { get; private set; }

        /// <param id="id"></param>
        /// <exception cref="ArgumentOutOfRangeException" />
        public void UpdateId(int id)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(id, nameof(id));
            Id = id;
        }

        /// <param name="name"></param>
        /// <exception cref="ArgumentException" />
        public void UpdateName(string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
            Name = name;
        }

        /// <param email="email"></param>
        /// <exception cref="ArgumentException" />
        public void UpdateEmail(string email)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(email, nameof(email));
            Email = email;
        }
    }
}
