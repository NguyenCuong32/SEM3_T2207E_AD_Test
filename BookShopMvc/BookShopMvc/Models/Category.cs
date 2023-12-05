using System;
using System.ComponentModel.DataAnnotations;

namespace BookShopMvc.Models
{
	public class Category
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		public string Slug { get; set; }
		public string Icon { get; set; }
		public int Status { get; set; }
		public virtual ICollection<Subcategory> Subcategories { get; set; }
	}
}

