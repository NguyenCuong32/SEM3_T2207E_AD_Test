using System;
using System.ComponentModel.DataAnnotations;

namespace BookShopMvc.Models
{
	public class Review
	{
		[Key]
		public int Id { get; set; }
		public int ProductID { get; set; }
        public virtual Product Product { get; set; }
        public int UserId { get; set; }
        public virtual Users User { get; set; }
        public string Comment { get; set; }
        public int Star_rate { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Update_at { get; set; }
    }
}

