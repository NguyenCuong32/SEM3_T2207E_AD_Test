using System;
using System.ComponentModel.DataAnnotations;

namespace BookShopMvc.Models
{
	public class Author
	{
		[Key]
		public int  Id { get; set; }
		[Required(ErrorMessage ="Input this name")]
		public string Name { get; set; }
		public string Address { get; set; }
		public string Slug { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}

