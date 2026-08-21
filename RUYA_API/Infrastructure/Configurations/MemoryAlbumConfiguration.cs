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

            builder.Property(m => m.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(m => m.StartDate)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(m => m.CoverImage)
                .HasMaxLength(500);

            builder.Property(m => m.SummaryText)
                .HasColumnType("text");

            builder.Property(m => m.TourId)
                .IsRequired(false);

            // MemoryAlbum -> AlbumItems: cascade (an album item has no meaning without its album)
            builder.HasMany(m => m.AlbumItems)
                .WithOne(ai => ai.MemoryAlbum)
                .HasForeignKey(ai => ai.AlbumId)
                .OnDelete(DeleteBehavior.Cascade);

            // UserId FK relationship configured from User side
            // TourId FK relationship configured from Tour side with SetNull behavior
        }
    }
}
