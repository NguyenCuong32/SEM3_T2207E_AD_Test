using System.ComponentModel.DataAnnotations.Schema;

namespace Qotos.Models
{
	public class PhotoAPI
	{
		public string Id { get; set; } = Guid.NewGuid().ToString();
		public string Thumbnail { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public string UserId { get; set; } = string.Empty;
		public DateTime Publish { get; set; } = DateTime.Now;
		public string Camera { get; set; } = string.Empty;
		public int Views { get; set; }
		// public int Likes { get; set; }
		// public int Downloads { get; set; }
		public string Location { get; set; } = string.Empty;
		public string Extension { get; set; } = string.Empty;

		public virtual AppUser? User { get; set; }
		public virtual ICollection<PhotoTag>? PhotoTags { get; set; }
		public virtual ICollection<Like>? Likes { get; set; }

		[NotMapped]
		public virtual List<string>? Tags { get; set; }

		[NotMapped]
		public IFormFile? FileImg { get; set; }
	}
}
