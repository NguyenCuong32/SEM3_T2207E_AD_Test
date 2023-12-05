using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BookShopMvc.Models
{
	public class Product
    {
        [Key]
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
        [Column(TypeName = "decimal(14,0)")]
        public decimal Price { get; set; }
        [Column(TypeName = "ntext")]
        public string Description { get; set; }
        public int Qty { get; set; }
        public int SubcategoryId  { get; set; }
        [JsonIgnore]
        public virtual Subcategory Subcategory { get; set; }
        public int AuthorId { get; set; }
        public virtual Author Author { get; set; }
        public int PublisherId { get; set; }
        public virtual Publisher Publisher { get; set; }

        public string Slug { get; set; }
        public int Status { get; set; }
        public string Thumb { get; set; }
        [NotMapped]
        [FileExtensions]
        public IFormFile ImageUpload { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Update_at { get; set; }

        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Order_item> Order_items { get; set; }
    }
}

