using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RUYA_API.Domain.Entities;

namespace RUYA_API.Infrastructure.Persistence.Configurations
{
    public class SourceConfiguration : IEntityTypeConfiguration<Source>
    {
        public void Configure(EntityTypeBuilder<Source> builder)
        {
            builder.ToTable("Sources");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(s => s.Type)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.Url)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(s => s.TrustLevel)
                .IsRequired()
                .HasMaxLength(50);

            // The Artifact <-> Source many-to-many join is configured
            // on ArtifactConfiguration to avoid defining it twice.
        }
    }
}
