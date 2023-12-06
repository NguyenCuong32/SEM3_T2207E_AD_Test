using System.ComponentModel.DataAnnotations;

namespace Qotos.Models.Authentication
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "*Tên không được để trống.")]
        public string FirstName { get; set; } = string.Empty;
        [Required(ErrorMessage = "*Họ không được để trống.")]
        public string LastName   { get; set; } = string.Empty;
        [EmailAddress(ErrorMessage = "*Email không đúng định dạng.")]
        [Required(ErrorMessage = "*Email không được để trống.")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "*Mật khẩu không được để trống.")]
        public string Password { get; set; } = string.Empty;
    }
}
