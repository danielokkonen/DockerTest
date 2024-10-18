using System.ComponentModel.DataAnnotations;

namespace DockerTest.Persistence.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public required string Name { get; set; }

        [MaxLength(100)]
        public required string Email { get; set; }
    }
}
