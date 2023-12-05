using System;
using System.ComponentModel.DataAnnotations;

namespace BookShopMvc.Models
{
	public class Cart
	{
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual Users User { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int Qty { get; set; }
	}
}

