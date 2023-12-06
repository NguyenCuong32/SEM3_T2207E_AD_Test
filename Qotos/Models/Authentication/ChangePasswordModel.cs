using System.ComponentModel.DataAnnotations;

namespace Qotos.Models.Authentication
{
	public class ChangePasswordModel
	{
		[Required(ErrorMessage = "*Mật khẩu không được để trống.")]
		[StringLength(6, ErrorMessage = "*Tối thiểu 6 ký tự.")]
		public string Password { get; set; } = string.Empty;

		[Required(ErrorMessage = "*Mật khẩu mới không được để trống.")]
		[StringLength(6, ErrorMessage = "*Tối thiểu 6 ký tự.")]
		public string NewPassword { get; set; } = string.Empty;

		[Required(ErrorMessage = "*Mật khẩu mới không được để trống.")]
		[StringLength(6, ErrorMessage = "*Tối thiểu 6 ký tự.")]
		[Compare(nameof(NewPassword), ErrorMessage = "*Mật khẩu không trùng nhau.")]
		public string RepeatNewPassword { get; set; } = string.Empty;
	}
}
