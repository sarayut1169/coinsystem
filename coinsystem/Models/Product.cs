using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace coinsystem.Models
{
    [Table("product")]

    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // กำหนดค่า Default สำหรับ Name ถ้าค่าเป็น NULL
        public string Name { get; set; } = string.Empty; // กำหนดค่าเริ่มต้นเป็น string ว่าง

        public double Price { get; set; }
        public int Amount { get; set; }

        // กำหนดค่า Default สำหรับ Picture ถ้าค่าเป็น NULL
        public string Picture { get; set; } = string.Empty; // กำหนดค่าเริ่มต้นเป็น string ว่าง
    }

    // public class ProductDTO
    // {
    //     public int Id { get; set; }
    //     public string Name { get; set; } = string.Empty;
    //     public double Price { get; set; }
    //     public int Amount { get; set; }
    // }
}