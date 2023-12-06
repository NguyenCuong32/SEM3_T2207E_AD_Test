using System.ComponentModel.DataAnnotations;

namespace Qotos.Models.Authentication
{
	public class LoginModel
	{
		[EmailAddress(ErrorMessage = "*Email không đúng định dạng.")]
		[Required(ErrorMessage = "*Email không được để trống.")]
		public string Email { get; set; } = string.Empty;

		[Required(ErrorMessage = "*Mật khẩu không được để trống.")]
		public string Password { get; set; } = string.Empty;
		public bool RememberMe { get; set; }
	}
}
