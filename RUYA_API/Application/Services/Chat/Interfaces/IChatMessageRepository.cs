using RUYA_API.Application.Services.Chat.DTOs;
using RUYA_API.Domain.Entities;

namespace RUYA_API.Application.Services.Chat.Interfaces
{
    public interface IChatMessageRepository
    {
        Task<List<ChatMessageHistoryDto>> GetRecentMessagesAsync(int conversationId, int count);
        Task<Message> AddMessageAsync(Message message);
        Task AddAttachmentAsync(ConversationAttachment attachment);
    }
}
