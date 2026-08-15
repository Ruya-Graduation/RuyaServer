using RUYA_API.Application.Services.Chat.DTOs;

namespace RUYA_API.Application.Services.Chat.Interfaces
{
    public interface IChatAiClient
    {
        Task<ConversationApiResponseDto> GetAiResponseAsync(ConversationApiRequestDto request, CancellationToken cancellationToken = default);
    }
}
