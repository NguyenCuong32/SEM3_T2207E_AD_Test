using System;
namespace BookShopMvc.Models
{
	public class FilteredProducts
	{
        public virtual Product Products { get; set; }
        public List<Publisher> Publishers { get; set; }
        public string PriceMin { get; set; }
        public string PriceMax { get; set; }
        public string Author { get; set; }
        public int PublisherId { get; set; }
    }
}

