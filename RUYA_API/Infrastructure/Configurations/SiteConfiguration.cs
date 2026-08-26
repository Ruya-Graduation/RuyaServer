using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RUYA_API.Domain.Entities;

namespace RUYA_API.Infrastructure.Configurations
{
    public class SiteConfiguration : IEntityTypeConfiguration<Site>
    {
        public void Configure(EntityTypeBuilder<Site> builder)
        {
            builder.ToTable("Sites");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Latitude)
                .IsRequired();

            builder.Property(s => s.Longitude)
                .IsRequired();

            builder.Property(s => s.ImageUrl)
                .HasMaxLength(500);

            builder.Property(s => s.ImagePublicId)
                .HasMaxLength(300);

            // Site -> Translations: cascade (translations have no meaning without their site)
            builder.HasMany(s => s.Translations)
                .WithOne(st => st.Site)
                .HasForeignKey(st => st.SiteId)
                .OnDelete(DeleteBehavior.Cascade);

            // Site -> Artifacts: cascade (artifact has no meaning without its site)
            builder.HasMany(s => s.Artifacts)
                .WithOne(a => a.Site)
                .HasForeignKey(a => a.SiteId)
                .OnDelete(DeleteBehavior.Cascade);

            // Site -> Tours: restrict (don't wipe tour history by deleting a site)
            builder.HasMany(s => s.Tours)
                .WithOne(t => t.Site)
                .HasForeignKey(t => t.SiteId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
