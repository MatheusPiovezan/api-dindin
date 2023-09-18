using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_dindin.Models
{
    [Table("usuarios")]
    public class User
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("nome")]
        public string Name { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("senha")]
        public string Password { get; set; }

        public User(string name, string email, string password)
        {
            Name = name;
            Email = email;
            Password = password;
        }
    }
}
