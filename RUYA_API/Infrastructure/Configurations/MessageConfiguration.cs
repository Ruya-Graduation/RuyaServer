using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RUYA_API.Domain.Entities;

namespace RUYA_API.Infrastructure.Persistence.Configurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("Messages");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Sender)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(m => m.AgentType)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(m => m.Content)
                .IsRequired()
                .HasColumnType("text"); // conversation content can run long, don't cap it artificially

            builder.Property(m => m.Timestamp)
                .IsRequired();

            // ConversationId FK relationship is configured from the Conversation side.
        }
    }
}
