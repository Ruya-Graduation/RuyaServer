using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RUYA_API.Domain.Entities;

namespace RUYA_API.Infrastructure.Persistence.Configurations
{
    public class TourStopConfiguration : IEntityTypeConfiguration<TourStop>
    {
        public void Configure(EntityTypeBuilder<TourStop> builder)
        {
            builder.ToTable("TourStops");

            builder.HasKey(ts => ts.Id);

            builder.Property(ts => ts.VisitedAt)
                .IsRequired();

            // TourId and ArtifactId relationships are configured
            // from TourConfiguration and ArtifactConfiguration.
        }
    }
}
