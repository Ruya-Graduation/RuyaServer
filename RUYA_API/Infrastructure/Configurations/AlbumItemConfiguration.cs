using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RUYA_API.Domain.Entities;

namespace RUYA_API.Infrastructure.Configurations
{
    public class AlbumItemConfiguration : IEntityTypeConfiguration<AlbumItem>
    {
        public void Configure(EntityTypeBuilder<AlbumItem> builder)
        {
            builder.ToTable("AlbumItems");

            builder.HasKey(ai => ai.Id);

            builder.Property(ai => ai.PhotoUrl)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(ai => ai.PublicId)
                .HasMaxLength(300);

            builder.Property(ai => ai.Caption)
                .HasMaxLength(500);

            builder.Property(ai => ai.DayLabel)
                .HasMaxLength(50);

            builder.Property(ai => ai.AiSummary)
                .HasColumnType("text");

            builder.Property(ai => ai.ArtifactId)
                .IsRequired(false);

            // AlbumId FK relationship configured from MemoryAlbum side
            // ArtifactId FK relationship configured from Artifact side with SetNull behavior
        }
    }
}
