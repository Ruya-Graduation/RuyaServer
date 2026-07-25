using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RUYA_API.Domain.Entities;

namespace RUYA_API.Infrastructure.Persistence.Configurations
{
    public class AlbumItemConfiguration : IEntityTypeConfiguration<AlbumItem>
    {
        public void Configure(EntityTypeBuilder<AlbumItem> builder)
        {
            builder.ToTable("AlbumItems");

            builder.HasKey(ai => ai.Id);

            builder.Property(ai => ai.PhotoUrl)
                .HasMaxLength(500);

            builder.Property(ai => ai.AiSummary)
                .HasColumnType("text");

            // AlbumId FK relationship is configured from the MemoryAlbum side.
            // ArtifactId FK relationship is configured from the Artifact side.
        }
    }
}
