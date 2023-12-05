using System;
using System.ComponentModel.DataAnnotations;

namespace BookShopMvc.Models
{
	public class Subcategory
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		public string Slug { get; set; }
        public int Status { get; set; }
        public int CategoryId { get; set; }
		public virtual Category Category { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}

