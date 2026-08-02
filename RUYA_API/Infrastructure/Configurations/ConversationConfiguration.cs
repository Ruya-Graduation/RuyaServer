using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RUYA_API.Domain.Entities;

namespace RUYA_API.Infrastructure.Persistence.Configurations
{
    public class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
    {
        public void Configure(EntityTypeBuilder<Conversation> builder)
        {
            builder.ToTable("Conversations");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Title)
                .HasMaxLength(200);

            builder.Property(c => c.Status)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.CurrentLanguage)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(c => c.CurrentMode)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.ModelName)
                .HasMaxLength(100);

            builder.Property(c => c.Summary)
                .HasColumnType("text");

            builder.Property(c => c.LastMessageAt)
                .IsRequired();

            builder.HasOne(c => c.User)
                .WithMany(u => u.Conversations)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(c => c.CurrentArtifact)
                .WithMany(a => a.Conversations)
                .HasForeignKey(c => c.CurrentArtifactId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(c => c.Messages)
                .WithOne(m => m.Conversation)
                .HasForeignKey(m => m.ConversationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Attachments)
                .WithOne(a => a.Conversation)
                .HasForeignKey(a => a.ConversationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
