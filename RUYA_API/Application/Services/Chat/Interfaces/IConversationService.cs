using RUYA_API.Application.Services.Chat.DTOs;

namespace RUYA_API.Application.Services.Chat.Interfaces
{
    public interface IConversationService
    {
        Task<ChatResponseDto> ChatAsync(ChatRequestDto dto);

        Task<ConversationDto?> GetConversationAsync(int conversationId);

        Task<IEnumerable<MessageDto>> GetMessagesAsync(int conversationId);

        Task<IEnumerable<ConversationDto>> GetUserConversationsAsync(string userId);

        Task DeleteConversationAsync(int conversationId);
    }
}
