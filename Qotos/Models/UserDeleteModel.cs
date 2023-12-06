using System.ComponentModel.DataAnnotations;

namespace Qotos.Models
{
	public class UserDeleteModel
	{
		[Required(ErrorMessage = "*Tên tài khoản không được để trống.")]
		public string UserName { get; set; } = string.Empty;
	}
}
