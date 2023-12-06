namespace Qotos.Models
{
	public class PhotoCollection
	{
		public string PhotoId { get; set; } = string.Empty;
		public virtual Photo? Photo { get; set; }
		public string CollectionId { get; set; } = string.Empty;
		public Collection? Collection { get; set; }
	}

}
