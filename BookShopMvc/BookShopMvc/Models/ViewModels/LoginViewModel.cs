using System;
using System.ComponentModel.DataAnnotations;

namespace BookShopMvc.Models.ViewModels
{
	public class LoginViewModel
	{
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please input UserName")]
        public string Username { get; set; }
        [DataType(DataType.Password), Required(ErrorMessage = "Please input password")]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}

