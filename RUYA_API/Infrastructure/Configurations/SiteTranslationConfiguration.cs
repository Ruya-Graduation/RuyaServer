using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RUYA_API.Domain.Entities;

namespace RUYA_API.Infrastructure.Configurations
{
    public class SiteTranslationConfiguration : IEntityTypeConfiguration<SiteTranslation>
    {
        public void Configure(EntityTypeBuilder<SiteTranslation> builder)
        {
            builder.ToTable("SiteTranslations");

            builder.HasKey(st => st.Id);

            builder.Property(st => st.LanguageCode)
                .IsRequired()
                .HasMaxLength(2);

            builder.Property(st => st.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(st => st.City)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(st => st.Country)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(st => st.Hours)
                .HasMaxLength(200);

            builder.Property(st => st.Ticket)
                .HasMaxLength(100);

            builder.Property(st => st.Crowds)
                .HasMaxLength(100);

            builder.Property(st => st.Description)
                .HasMaxLength(2000);

            // Unique constraint: one translation per language per site
            builder.HasIndex(st => new { st.SiteId, st.LanguageCode })
                .IsUnique();

            // Relationship with Site
            builder.HasOne(st => st.Site)
                .WithMany(s => s.Translations)
                .HasForeignKey(st => st.SiteId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
