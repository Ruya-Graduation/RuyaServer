using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RUYA_API.Domain.Entities;

namespace RUYA_API.Infrastructure.Configurations
{
    public class ArtifactConfiguration : IEntityTypeConfiguration<Artifact>
    {
        public void Configure(EntityTypeBuilder<Artifact> builder)
        {
            builder.ToTable("Artifacts");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.ImagePublicId)
                .HasMaxLength(300);

            builder.Property(a => a.ImageUrl)
                .HasMaxLength(500);

            // Artifact -> Translations: cascade (translations have no meaning without their artifact)
            builder.HasMany(a => a.Translations)
                .WithOne(at => at.Artifact)
                .HasForeignKey(at => at.ArtifactId)
                .OnDelete(DeleteBehavior.Cascade);

            // Artifact -> TourStops: restrict (deleting an artifact shouldn't erase visit history)
            builder.HasMany(a => a.TourStops)
                .WithOne(ts => ts.Artifact)
                .HasForeignKey(ts => ts.ArtifactId)
                .OnDelete(DeleteBehavior.Restrict);

            // Artifact -> AlbumItems: set null (photos can exist without artifact reference)
            builder.HasMany(a => a.AlbumItems)
                .WithOne(ai => ai.Artifact)
                .HasForeignKey(ai => ai.ArtifactId)
                .OnDelete(DeleteBehavior.SetNull);

            // Artifact <-> Source: many-to-many (verified_by), implicit join table "ArtifactSources"
            builder.HasMany(a => a.Sources)
                .WithMany(s => s.Artifacts)
                .UsingEntity(j => j.ToTable("ArtifactSources"));
        }
    }
}
