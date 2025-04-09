using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace coinsystem.Models
{
    [Table("money")]
    public class Money
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; }
        public int Amount { get; set; }
    }
    
    public class MoneyDTO
    {
        // public int Id { get; set; }
        // public string Name { get; set; } = string.Empty;
        public double Price { get; set; }
        public int Amount { get; set; }
    }
}