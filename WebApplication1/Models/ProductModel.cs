using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class ProductModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Images { get; set; }
        [DefaultValue(0.0f)]
        [Required]
        public float Price { get; set; }
        [DefaultValue(0.0)]
        [Required]
        public int Quantity { get; set; }
        //ForeignKey
        [ForeignKey("Seasons")]
        public int seasonid { get; set; }
        public virtual SeasonModel Seasons { get; set; }
        //ForeignKey
        [ForeignKey("Genders")]
        public int GendersId { get; set; }
        public virtual GendersModel Genders { get; set; }
        //ForeignKey
        [Required]
        [ForeignKey("Ages")]
        public int Age { get; set; }
        public virtual AgesModel Ages { get; set; }
        [Required]
        public float RewardPoint { get; set; }
    }
}
