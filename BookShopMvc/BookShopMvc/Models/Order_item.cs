using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookShopMvc.Models
{
	public class Order_item
	{
		[Key]
		public int Id { get; set; }
		public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
        public int Buy_qty { get; set; }
		[Column(TypeName = "decimal(14,0)")]
		public decimal Price { get; set; }
		public string Slug { get; set; }
	}
}

