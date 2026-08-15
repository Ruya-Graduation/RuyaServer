using Microsoft.EntityFrameworkCore;
using RUYA_API.Application.Services.Chat.DTOs;
using RUYA_API.Application.Services.Chat.Interfaces;
using RUYA_API.Domain.Entities;
using RUYA_API.ExceptionHandling.CustomException;
using RUYA_API.Infrastructure.Context;

namespace RUYA_API.Application.Services.Chat.Service
{
    public class ConversationService : IConversationService
    {
        private readonly RuyaContext _context;

        public ConversationService(RuyaContext context)
        {
            _context = context;
        }

        public async Task<ChatResponseDto> ChatAsync(ChatRequestDto dto)
        {
            Conversation conversation;

            if (dto.ConversationId.HasValue)
            {
                conversation = await _context.Conversations
                    .FirstOrDefaultAsync(c => c.Id == dto.ConversationId.Value)
                    ?? throw new AppException("Conversation not found.", StatusCodes.Status404NotFound);
            }
            else
            {
                conversation = new Conversation
                {
                    Title = "New Conversation",
                    Status = "Active",
                    CurrentLanguage = dto.Language,
                    CurrentMode = dto.Mode,
                    LastMessageAt = DateTime.UtcNow
                };

                _context.Conversations.Add(conversation);
                await _context.SaveChangesAsync();
            }

            var userMessage = new Message
            {
                ConversationId = conversation.Id,
                Role = "user",
                Content = dto.Message
            };

            _context.Messages.Add(userMessage);

            conversation.LastMessageAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new ChatResponseDto
            {
                ConversationId = conversation.Id,
                AssistantMessage = "AI integration is not connected yet.",
                CurrentArtifactId = conversation.CurrentArtifactId,
                UsedVision = false,
                NeedsNewFrame = false
            };
        }

        public async Task<ConversationDto?> GetConversationAsync(int conversationId)
        {
            var conversation = await _context.Conversations
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == conversationId);

            if (conversation == null)
                throw new AppException("Conversation not found.", StatusCodes.Status404NotFound);

            return new ConversationDto
            {
                Id = conversation.Id,
                Title = conversation.Title,
                Status = conversation.Status,
                CurrentLanguage = conversation.CurrentLanguage,
                CurrentMode = conversation.CurrentMode,
                LastMessageAt = conversation.LastMessageAt
            };
        }

        public async Task<IEnumerable<MessageDto>> GetMessagesAsync(int conversationId)
        {
            var exists = await _context.Conversations
                .AnyAsync(c => c.Id == conversationId);

            if (!exists)
                throw new AppException("Conversation not found.", StatusCodes.Status404NotFound);

            return await _context.Messages
                .Where(m => m.ConversationId == conversationId)
                .OrderBy(m => m.CreatedAt)
                .Select(m => new MessageDto
                {
                    Id = m.Id,
                    Role = m.Role,
                    Content = m.Content,
                    CreatedAt = m.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<ConversationDto>> GetUserConversationsAsync(string userId)
        {
            return await _context.Conversations
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.LastMessageAt)
                .Select(c => new ConversationDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Status = c.Status,
                    CurrentLanguage = c.CurrentLanguage,
                    CurrentMode = c.CurrentMode,
                    LastMessageAt = c.LastMessageAt
                })
                .ToListAsync();
        }

        public async Task DeleteConversationAsync(int conversationId)
        {
            var conversation = await _context.Conversations
                .FirstOrDefaultAsync(c => c.Id == conversationId);

            if (conversation == null)
                throw new AppException("Conversation not found.", StatusCodes.Status404NotFound);

            _context.Conversations.Remove(conversation);

            await _context.SaveChangesAsync();
        }
    }
}
