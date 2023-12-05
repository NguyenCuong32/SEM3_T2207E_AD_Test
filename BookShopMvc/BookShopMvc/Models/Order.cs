using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShopMvc.Models
{
	public class Order
	{
		[Key]
		public int Id { get; set; }
		public int UserId { get; set; }
        public virtual Users User { get; set; }
        public string Fullname { get; set; }
		public string Phone { get; set; }
		public string Shipping_address { get; set; }
		public string City { get; set; }
        [Column(TypeName = "decimal(14,0)")]
        public decimal Grand_total { get; set; }
		public int Status { get; set; }
		public string Slug { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Update_at { get; set; }

        public virtual ICollection<Order_item> Order_items { get; set; }

    }
}

