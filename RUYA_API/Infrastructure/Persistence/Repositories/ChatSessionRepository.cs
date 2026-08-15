using Microsoft.EntityFrameworkCore;
using RUYA_API.Application.Services.Chat.Interfaces;
using RUYA_API.Domain.Entities;
using RUYA_API.Infrastructure.Context;

namespace RUYA_API.Infrastructure.Persistence.Repositories
{
    public class ChatSessionRepository : IChatSessionRepository
    {
        private readonly RuyaContext _context;

        public ChatSessionRepository(RuyaContext context)
        {
            _context = context;
        }

        public async Task<Conversation?> GetByIdAsync(int conversationId)
        {
            return await _context.Conversations
                .Include(c => c.Messages)
                .Include(c => c.Attachments)
                .FirstOrDefaultAsync(c => c.Id == conversationId);
        }

        public async Task<Conversation> CreateAsync(Conversation conversation)
        {
            _context.Conversations.Add(conversation);
            await _context.SaveChangesAsync();
            return conversation;
        }

        public async Task UpdateAsync(Conversation conversation)
        {
            _context.Conversations.Update(conversation);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Conversation>> GetUserConversationsAsync(string userId)
        {
            return await _context.Conversations
                .AsNoTracking()
                .Include(c => c.Messages)
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.LastMessageAt)
                .ToListAsync();
        }

        public async Task DeleteAsync(Conversation conversation)
        {
            _context.ConversationAttachments.RemoveRange(conversation.Attachments);
            _context.Messages.RemoveRange(conversation.Messages);
            _context.Conversations.Remove(conversation);
            await _context.SaveChangesAsync();
        }
    }
}
