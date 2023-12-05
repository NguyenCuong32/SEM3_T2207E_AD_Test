using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class DetailOrderModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DetailId { get; set; }

        [Required]
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual ProductModel Product { get; set; }
        [Required]
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public virtual OrderModel Order { get; set; }
        

        [DefaultValue(0.0f)]
        [Required]
        public float Money { get; set; }

        [DefaultValue(0.0)]
        [Required]
        public int Quantity { get; set; }
    }
}
