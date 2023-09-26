using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_dindin.Models
{
    [Table("transactions")]
    public class Transaction
    {
        [Key]
        public int id { get; set; }
        public string description { get; set; }
        public double value { get; set; }
        public DateTime date { get; set; }
        public string type { get; set; }
        public int category_id { get; set; }
        public int user_id { get; set; }

        public Transaction(string description, double value, DateTime date, string type, int category_id, int user_id)
        {
            this.description = description;
            this.value = value;
            this.date = date;
            this.type = type;
            this.category_id = category_id;
            this.user_id = user_id;
        }
    }
}
