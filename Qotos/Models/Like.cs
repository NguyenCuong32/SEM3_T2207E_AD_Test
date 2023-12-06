namespace Qotos.Models
{
	public class Like
	{
		public string PhotoId { get; set; } = string.Empty;
		public virtual Photo? Photo { get; set; }
		public string UserId { get; set; } = string.Empty;
		public AppUser? User { get; set; }
	}

}