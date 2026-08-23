using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RUYA_API.Domain.Entities;

namespace RUYA_API.Infrastructure.Configurations
{
    public class ArtifactTranslationConfiguration : IEntityTypeConfiguration<ArtifactTranslation>
    {
        public void Configure(EntityTypeBuilder<ArtifactTranslation> builder)
        {
            builder.ToTable("ArtifactTranslations");

            builder.HasKey(at => at.Id);

            builder.Property(at => at.LanguageCode)
                .IsRequired()
                .HasMaxLength(2);

            builder.Property(at => at.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(at => at.Category)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(at => at.Civilization)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(at => at.Period)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(at => at.Material)
                .HasMaxLength(200);

            builder.Property(at => at.PlaceOfDiscovery)
                .HasMaxLength(200);

            // Unique constraint: one translation per language per artifact
            builder.HasIndex(at => new { at.ArtifactId, at.LanguageCode })
                .IsUnique();

            // Relationship with Artifact
            builder.HasOne(at => at.Artifact)
                .WithMany(a => a.Translations)
                .HasForeignKey(at => at.ArtifactId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
