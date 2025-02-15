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
        public string Name { get; private set; }
        public string Email { get; private set; }

        /// <summary>
        /// Update user name.
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="ArgumentException" />
        public void UpdateName(string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
            Name = name;
        }

        /// <summary>
        /// Update user email.
        /// </summary>
        /// <param email="email"></param>
        /// <exception cref="ArgumentException" />
        public void UpdateEmail(string email)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(email, nameof(email));
            Email = email;
        }
    }
}
