namespace Qotos.Models
{
	public class Collection
	{
		public string Id { get; set; } = Guid.NewGuid().ToString();
		public string Title { get; set; } = string.Empty;

		public DateTime CreatedAt { get; set; } = DateTime.Now;

		public string UserId { get; set; } = string.Empty;
		public virtual AppUser? User { get; set; }
		public virtual ICollection<PhotoCollection>? PhotoCollections { get; set; }

	}

}