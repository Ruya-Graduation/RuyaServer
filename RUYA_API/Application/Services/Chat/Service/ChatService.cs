using Microsoft.EntityFrameworkCore;
using RUYA_API.Application.Common.Interfaces;
using RUYA_API.Application.Services.Chat.DTOs;
using RUYA_API.Application.Services.Chat.Interfaces;
using RUYA_API.Domain.Entities;
using RUYA_API.ExceptionHandling.CustomException;
using RUYA_API.Infrastructure.Context;

namespace RUYA_API.Application.Services.Chat.Service
{
    public class ChatService : IChatService
    {
        private readonly RuyaContext _context;
        private readonly IImageService _imageService;
        private readonly IAIService _aiService;

        public ChatService(RuyaContext context, IImageService imageService, IAIService aiService)
        {
            _context = context;
            _imageService = imageService;
            _aiService = aiService;
        }

        public async Task<ChatResponseDto> SendMessageAsync(ChatRequestDto dto, string? userId)
        {
            Conversation conversation;

            if (dto.ConversationId.HasValue)
            {
                conversation = await _context.Conversations
                    .FirstOrDefaultAsync(c => c.Id == dto.ConversationId.Value);

                if (conversation is null)
                    throw new AppException(
                        "Conversation not found.",
                        StatusCodes.Status404NotFound);
            }
            else
            {
                // TODO:
                // When JWT authentication is fully integrated, the authenticated user's Id
                // will be stored here instead of relying on the current placeholder value.
                conversation = new Conversation
                {
                    UserId = userId,
                    Title = dto.Message.Length > 50 ? dto.Message[..50] + "..." : dto.Message,
                    Status = "Active",
                    CurrentLanguage = dto.Language,
                    CurrentMode = dto.Mode,
                    LastMessageAt = DateTime.UtcNow
                };

                _context.Conversations.Add(conversation);
                await _context.SaveChangesAsync();
            }

            var message = new Message
            {
                ConversationId = conversation.Id,
                Role = "user",
                Content = dto.Message
            };

            _context.Messages.Add(message);

            conversation.LastMessageAt = DateTime.UtcNow;
            conversation.CurrentLanguage = dto.Language;
            conversation.CurrentMode = dto.Mode;

            await _context.SaveChangesAsync();

            if (dto.Image != null)
            {
                var image = await _imageService.UploadImageAsync(dto.Image);

                var attachment = new ConversationAttachment
                {
                    ConversationId = conversation.Id,
                    MessageId = message.Id,
                    FileUrl = image.ImageUrl,
                    PublicId = image.PublicId,
                    FileType = "image",
                    MimeType = dto.Image.ContentType,
                    IsPrimaryFrame = true
                };

                _context.ConversationAttachments.Add(attachment);

                await _context.SaveChangesAsync();
            }

            var aiResponse = await _aiService.SendMessageAsync(dto);

            aiResponse.ConversationId = conversation.Id;
            aiResponse.CurrentArtifactId = conversation.CurrentArtifactId;

            return aiResponse;
        }

        public async Task<ConversationDetailsDto> GetConversationAsync(int conversationId)
        {
            // TODO:
            // Currently conversations are filtered using the provided userId.
            // After authentication is completed, this value will come directly
            // from the authenticated user's JWT claims.
            var conversation = await _context.Conversations
                .Include(c => c.Messages)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == conversationId);

            if (conversation is null)
            {
                throw new AppException(
                    "Conversation not found.",
                    StatusCodes.Status404NotFound);
            }

            return new ConversationDetailsDto
            {
                ConversationId = conversation.Id,
                Title = conversation.Title,
                Status = conversation.Status,
                CurrentLanguage = conversation.CurrentLanguage,
                CurrentMode = conversation.CurrentMode,
                LastMessageAt = conversation.LastMessageAt,
                Messages = conversation.Messages
                    .OrderBy(m => m.CreatedAt)
                    .Select(m => new MessageDto
                    {
                        Id = m.Id,
                        Role = m.Role,
                        Content = m.Content,
                        CreatedAt = m.CreatedAt
                    })
                    .ToList()
            };
        }

        public async Task<IEnumerable<ConversationListItemDto>> GetConversationsAsync(string? userId)
        {
            var conversations = await _context.Conversations
                .AsNoTracking()
                .Include(c => c.Messages)
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.LastMessageAt)
                .ToListAsync();

            return conversations.Select(c => new ConversationListItemDto
            {
                ConversationId = c.Id,
                Title = c.Title,
                Status = c.Status,
                LastMessageAt = c.LastMessageAt,
                MessageCount = c.Messages.Count
            });
        }

        public async Task DeleteConversationAsync(int conversationId, string? userId)
        {
            var conversation = await _context.Conversations
                .Include(c => c.Messages)
                .Include(c => c.Attachments)
                .FirstOrDefaultAsync(c => c.Id == conversationId);

            if (conversation is null)
            {
                throw new AppException(
                    "Conversation not found.",
                    StatusCodes.Status404NotFound);
            }

            // TODO:
            // Enable ownership validation after JWT authentication is completed.
            // The authenticated user's Id will be compared against Conversation.UserId.
            // if (conversation.UserId != userId)
            // {
            //     throw new AppException(
            //         "You are not allowed to delete this conversation.",
            //         StatusCodes.Status403Forbidden);
            // }

            _context.ConversationAttachments.RemoveRange(conversation.Attachments);

            _context.Messages.RemoveRange(conversation.Messages);

            _context.Conversations.Remove(conversation);

            await _context.SaveChangesAsync();
        }
    }
}
