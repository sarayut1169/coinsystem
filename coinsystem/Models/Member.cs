using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace coinsystem.Models
{
    [Table("member")]
    public class Member
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Tell{ get; set; } = string.Empty;
        public double Credit { get; set; }
    }
    
    // public class MemberDTO
    // {
    //     
    // }
}