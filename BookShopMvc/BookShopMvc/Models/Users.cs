using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BookShopMvc.Models
{
	public class Users
	{
		[Key]
		public int Id { get; set; }
		[Required(ErrorMessage = "Please input UserName")]
		public string Username { get; set; }
		[Required(ErrorMessage = "Please input Email"),EmailAddress]
		public string Email { get; set; }
		[DataType(DataType.Password),Required(ErrorMessage = "Please input password")]
		public string Password { get; set; }
		public string? Fullname { get; set; }
		public string? Phone { get; set; }
		public string? Address { get; set; }
		public string? Img { get; set; }
        public string? RoleId { get; set; }
        public DateTime Created_at { get; set; }
		public DateTime Update_at { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
		public List<IdentityRole> Roles { get; set; }
    }
}

