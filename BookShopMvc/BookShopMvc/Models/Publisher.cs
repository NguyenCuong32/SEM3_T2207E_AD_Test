using System;
using System.ComponentModel.DataAnnotations;

namespace BookShopMvc.Models
{
	public class Publisher
	{
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }
		public string Slug { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}

