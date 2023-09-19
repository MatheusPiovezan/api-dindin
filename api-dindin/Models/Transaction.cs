using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_dindin.Models
{
    [Table("transactions")]
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public double Value { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }
    }
}
