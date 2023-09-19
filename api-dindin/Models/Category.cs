using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_dindin.Models
{
    [Table("categories")]
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }

        public Category(int id, string description)
        {
            Description = description;
        }
    }
}
