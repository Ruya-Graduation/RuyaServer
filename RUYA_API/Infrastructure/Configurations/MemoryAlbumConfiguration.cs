using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RUYA_API.Domain.Entities;

namespace RUYA_API.Infrastructure.Configurations
{
    public class MemoryAlbumConfiguration : IEntityTypeConfiguration<MemoryAlbum>
    {
        public void Configure(EntityTypeBuilder<MemoryAlbum> builder)
        {
            builder.ToTable("MemoryAlbums");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.GeneratedAt)
                .IsRequired();

            builder.Property(m => m.SummaryText)
                .HasColumnType("text");

            builder.Property(m => m.Title)
                .HasMaxLength(200);

            builder.Property(m => m.Year)
                .IsRequired();

            builder.Property(m => m.CoverImage)
                .HasMaxLength(500);

            builder.Property(m => m.TourId)
                .IsRequired(false); // TourId is optional

            // MemoryAlbum -> AlbumItems: cascade (an album item has no meaning without its album)
            builder.HasMany(m => m.AlbumItems)
                .WithOne(ai => ai.MemoryAlbum)
                .HasForeignKey(ai => ai.AlbumId)
                .OnDelete(DeleteBehavior.Cascade);

            // UserId and TourId FK relationships are configured from
            // the User and Tour sides respectively.
        }
    }
}
