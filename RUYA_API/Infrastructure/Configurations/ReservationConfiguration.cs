using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RUYA_API.Domain.Entities;

namespace RUYA_API.Infrastructure.Configurations
{
    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.ToTable("Reservations");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.MuseumName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(r => r.ReservationDate)
                .IsRequired();

            builder.Property(r => r.UserId)
                .IsRequired()
                .HasMaxLength(450);

            // Reservation -> User: restrict (don't delete reservations if user is deleted)
            builder.HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
