namespace Qotos.Models
{
	public class Tag
	{
		public string Id { get; set; } = Guid.NewGuid().ToString();
		public string TagName { get; set; } = string.Empty;
		public int Searchs { get; set; } = 0;
		public ICollection<PhotoTag>? PhotoTags { get; set; }
	}
}
