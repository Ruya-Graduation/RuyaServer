using RUYA_API.Application.Services.Chat.DTOs;

namespace RUYA_API.Application.Common.Interfaces
{
    public interface IAIService
    {
        Task<ChatResponseDto> SendMessageAsync(ChatRequestDto request);
    }
}
