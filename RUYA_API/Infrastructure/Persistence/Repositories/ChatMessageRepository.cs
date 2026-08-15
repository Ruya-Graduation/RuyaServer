using Microsoft.EntityFrameworkCore;
using RUYA_API.Application.Services.Chat.DTOs;
using RUYA_API.Application.Services.Chat.Interfaces;
using RUYA_API.Domain.Entities;
using RUYA_API.Infrastructure.Context;

namespace RUYA_API.Infrastructure.Persistence.Repositories
{
    public class ChatMessageRepository : IChatMessageRepository
    {
        private readonly RuyaContext _context;

        public ChatMessageRepository(RuyaContext context)
        {
            _context = context;
        }

        public async Task<List<ChatMessageHistoryDto>> GetRecentMessagesAsync(int conversationId, int count)
        {
            var recentMessages = await _context.Messages
                .AsNoTracking()
                .Where(m => m.ConversationId == conversationId)
                .OrderByDescending(m => m.CreatedAt)
                .Take(count)
                .ToListAsync();

            return recentMessages
                .OrderBy(m => m.CreatedAt)
                .Select(m => new ChatMessageHistoryDto
                {
                    Role = m.Role,
                    Content = m.Content
                })
                .ToList();
        }

        public async Task<Message> AddMessageAsync(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task AddAttachmentAsync(ConversationAttachment attachment)
        {
            _context.ConversationAttachments.Add(attachment);
            await _context.SaveChangesAsync();
        }
    }
}
