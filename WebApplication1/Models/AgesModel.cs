using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class AgesModel
    {
        [Key]
        public int agesid { get; set; }
        [Required]
        [StringLength(50)]
        public string agenumber { get; set; }
        
    }
}
