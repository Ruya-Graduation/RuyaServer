using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RUYA_API.Domain.Entities;

namespace RUYA_API.Infrastructure.Configurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("Messages");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Role)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(m => m.Content)
                .IsRequired()
                .HasColumnType("text");

            builder.Property(m => m.ModelName)
                .HasMaxLength(100);

            builder.Property(m => m.Metadata)
                .HasColumnType("text");
        }
    }
}
