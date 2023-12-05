using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class SeasonModel
    {
        [Key]
        [Required]
        public int Seasonid { get; set; }
        [Required]
        [StringLength(50)]
        public string Season { get; set; }
    }
}
