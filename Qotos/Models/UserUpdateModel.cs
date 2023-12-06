using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Qotos.Models
{
	public class UserUpdateModel
	{
		[Required(ErrorMessage = "*Tên không được để trống.")]
		public string FirstName { get; set; } = string.Empty;
		[Required(ErrorMessage = "*Họ không được để trống.")]
		public string LastName { get; set; } = string.Empty;
		[EmailAddress(ErrorMessage = "*Email không đúng định dạng.")]
		[Required(ErrorMessage = "*Email không được để trống.")]
		public string Email { get; set; } = string.Empty;
		[Required(ErrorMessage = "*Tên đăng nhập không được để trống.")]
		public string UserName { get; set; } = string.Empty;
		public string Thumbnail { get; set; } = string.Empty;

		[NotMapped]
		public IFormFile? File { get; set; }
	}
}
