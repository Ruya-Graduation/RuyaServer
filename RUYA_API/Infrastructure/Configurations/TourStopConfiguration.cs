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

            // TourStop -> Conversations: cascade (a conversation is tied to that specific stop)
            builder.HasMany(ts => ts.Conversations)
                .WithOne(c => c.TourStop)
                .HasForeignKey(c => c.StopId)
                .OnDelete(DeleteBehavior.Cascade);

            // TourId and ArtifactId FK relationships are configured from
            // the Tour and Artifact sides respectively.
        }
    }
}
