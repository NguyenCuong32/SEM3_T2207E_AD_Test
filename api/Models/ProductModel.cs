using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace api.Models
{
    public class ProductModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }
        [Column(TypeName = "nvarchar(512)")]
        public string Name { get; set; }
        [Column(TypeName = "nvarchar(512)")]
        public string Images { get; set; }

        [DefaultValue(0.0f)]
        [Required]
        public float Price { get; set; }

        [DefaultValue(0.0)]
        [Required]
        public int Quantity { get; set; }

        [Column(TypeName = "nvarchar(512)")]

        public string Seasons { get; set; }
        [Column(TypeName = "nvarchar(512)")]

        public string Genders { get; set; }
        [DefaultValue(0.0)]
        [Required]

        public int Ages { get; set; }

        [DefaultValue(0.0f)]
        [Required]
        public float RewardPoint { get; set; }
    }
}
