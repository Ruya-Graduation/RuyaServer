using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RUYA_API.Domain.Entities;

namespace RUYA_API.Infrastructure.Configurations
{
    public class TourConfiguration : IEntityTypeConfiguration<Tour>
    {
        public void Configure(EntityTypeBuilder<Tour> builder)
        {
            builder.ToTable("Tours");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.StartTime)
                .IsRequired();

            builder.Property(t => t.EndTime);

            builder.Property(t => t.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // Tour -> TourStops: cascade (a stop has no meaning without its tour)
            builder.HasMany(t => t.TourStops)
                .WithOne(ts => ts.Tour)
                .HasForeignKey(ts => ts.TourId)
                .OnDelete(DeleteBehavior.Cascade);

            // Tour -> MemoryAlbums: restrict
            builder.HasMany(t => t.MemoryAlbums)
                .WithOne(m => m.Tour)
                .HasForeignKey(m => m.TourId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
