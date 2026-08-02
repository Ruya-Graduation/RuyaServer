using RUYA_API.Application.Common.Interfaces;
using RUYA_API.Application.Services.Chat.DTOs;

namespace RUYA_API.Infrastructure.Services
{
    public class FakeAIService : IAIService
    {
        public Task<ChatResponseDto> SendMessageAsync(ChatRequestDto request)
        {
            return Task.FromResult(new ChatResponseDto
            {
                AssistantMessage = "AI integration is not connected yet.",
                UsedVision = request.Image != null,
                NeedsNewFrame = false
            });
        }
    }
}
