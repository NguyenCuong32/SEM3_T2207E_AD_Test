using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Qotos.Models;



namespace Qotos.Data
{
	public class ApplicationDbContext : IdentityDbContext<AppUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
				: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			// PhotoTags
			base.OnModelCreating(builder);
			builder.Entity<PhotoTag>()
			.HasKey(pt => new { pt.TagId, pt.PhotoId });

			builder.Entity<PhotoTag>()
					.HasOne(pt => pt.Photo)
					.WithMany(p => p.PhotoTags)
					.HasForeignKey(pt => pt.PhotoId);

			builder.Entity<PhotoTag>()
					.HasOne(pt => pt.Tag)
					.WithMany(t => t.PhotoTags)
					.HasForeignKey(pt => pt.TagId);

			// Likes
			builder.Entity<Like>()
			.HasKey(pt => new { pt.UserId, pt.PhotoId });

			builder.Entity<Like>()
					.HasOne(pt => pt.Photo)
					.WithMany(p => p.Likes)
					.HasForeignKey(pt => pt.PhotoId);

			builder.Entity<Like>()
					.HasOne(pt => pt.User)
					.WithMany(t => t.Likes)
					.HasForeignKey(pt => pt.UserId);

			//Data
			builder.Entity<AppUser>()
			.HasMany(u => u.Collections)
			.WithOne(c => c.User);

			// Download
			builder.Entity<UserDownload>()
			.HasKey(pt => new { pt.UserId, pt.PhotoId });

			builder.Entity<UserDownload>()
					.HasOne(pt => pt.User)
					.WithMany(p => p.UserDownloads)
					.HasForeignKey(pt => pt.UserId);

			builder.Entity<UserDownload>()
					.HasOne(pt => pt.Photo)
					.WithMany(t => t.UserDownloads)
					.HasForeignKey(pt => pt.PhotoId);


			//Collection - Photo
			builder.Entity<PhotoCollection>()
			.HasKey(pt => new { pt.CollectionId, pt.PhotoId });

			builder.Entity<PhotoCollection>()
					.HasOne(pt => pt.Photo)
					.WithMany(p => p.PhotoCollections)
					.HasForeignKey(pt => pt.PhotoId);

			builder.Entity<PhotoCollection>()
					.HasOne(pt => pt.Collection)
					.WithMany(t => t.PhotoCollections)
					.HasForeignKey(pt => pt.CollectionId);
		}

		public DbSet<Qotos.Models.Photo>? Photos { get; set; }
		public DbSet<Qotos.Models.Tag>? Tags { get; set; }
		public DbSet<Qotos.Models.PhotoTag>? PhotoTags { get; set; }
		public DbSet<Qotos.Models.Like>? Likes { get; set; }
		public DbSet<Qotos.Models.Collection>? Collections { get; set; }
		public DbSet<Qotos.Models.PhotoCollection>? PhotoCollections { get; set; }
		public DbSet<Qotos.Models.UserDownload>? UserDownloads { get; set; }
		public DbSet<Qotos.Models.Report>? Reports { get; set; }

	}
}
