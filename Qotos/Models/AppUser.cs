using Microsoft.AspNetCore.Identity;

namespace Qotos.Models
{
	public class AppUser : IdentityUser
	{
		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
		public string Thumbnail { get; set; } = "https://media-cdn-v2.laodong.vn/storage/newsportal/2021/5/13/909067/D1.jpg";
		public string Description { get; set; } = "Lối nhỏ - Đen vâu";
		public ICollection<Like>? Likes { get; set; }

		public ICollection<UserDownload>? UserDownloads { get; set; }
		public ICollection<Collection>? Collections { get; set; }

	}
}
