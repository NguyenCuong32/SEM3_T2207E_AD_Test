using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    

    public class GendersModel
    {
        [Key]
        [Required]
        public int Genderid { get; set; }
        [Required]
        [StringLength(50)]
        public string Gender { get; set; }

    }
}
