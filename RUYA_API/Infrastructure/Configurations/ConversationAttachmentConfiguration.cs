using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RUYA_API.Domain.Entities;

namespace RUYA_API.Infrastructure.Configurations
{
    public class ConversationAttachmentConfiguration : IEntityTypeConfiguration<ConversationAttachment>
    {
        public void Configure(EntityTypeBuilder<ConversationAttachment> builder)
        {
            builder.ToTable("ConversationAttachments");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.FileUrl)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(a => a.PublicId)
                .HasMaxLength(300);

            builder.Property(a => a.FileType)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(a => a.MimeType)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.VisionResultJson)
                .HasColumnType("text");

            builder.HasOne(a => a.Message)
                .WithMany()
                .HasForeignKey(a => a.MessageId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
