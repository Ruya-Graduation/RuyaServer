using RUYA_API.Application.Services.Chat.DTOs;

namespace RUYA_API.Application.Services.Chat.Interfaces
{
    public interface IChatService
    {
        Task<ChatResponseDto> SendMessageAsync(ChatRequestDto dto, string? userId);

        Task<ConversationDetailsDto> GetConversationAsync(int conversationId, string? userId);

        Task<IEnumerable<ConversationListItemDto>> GetConversationsAsync(string? userId);

        Task DeleteConversationAsync(int conversationId, string? userId);
    }
}
