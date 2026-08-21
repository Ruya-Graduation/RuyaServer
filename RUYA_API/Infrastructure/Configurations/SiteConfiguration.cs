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

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(s => s.City)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.Country)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.Latitude)
                .IsRequired();

            builder.Property(s => s.Longitude)
                .IsRequired();

            builder.Property(s => s.Hours)
                .HasMaxLength(200);

            builder.Property(s => s.Ticket)
                .HasMaxLength(100);

            builder.Property(s => s.Crowds)
                .HasMaxLength(100);

            builder.Property(s => s.Description)
                .HasMaxLength(2000);

            builder.Property(s => s.ImageUrl)
                .HasMaxLength(500);

            builder.Property(s => s.ImagePublicId)
                .HasMaxLength(300);

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
