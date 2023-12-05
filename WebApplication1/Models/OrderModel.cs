using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class OrderModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }
        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual UserModel Users { get; set; }
        public string Description { get; set; }
    }
}
