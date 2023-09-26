using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_dindin.Models
{
    [Table("categories")]
    public class Category
    {
        [Key]
        public int id { get; set; }
        public string description { get; set; }

        public Category(string description)
        {
            this.description = description;
        }
    }
}
