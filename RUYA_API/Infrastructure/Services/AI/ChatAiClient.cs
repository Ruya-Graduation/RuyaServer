using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RUYA_API.Application.Services.Chat.DTOs;
using RUYA_API.Application.Services.Chat.Interfaces;

namespace RUYA_API.Infrastructure.Services.AI
{
    public class ChatAiClient : IChatAiClient
    {
        private readonly HttpClient _httpClient;
        private readonly AIServiceSettings _settings;
        private readonly ILogger<ChatAiClient> _logger;

        public ChatAiClient(HttpClient httpClient, IOptions<AIServiceSettings> settings, ILogger<ChatAiClient> logger)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task<ConversationApiResponseDto> GetAiResponseAsync(ConversationApiRequestDto request, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(_settings.ChatEndpoint, request, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ConversationApiResponseDto>(cancellationToken: cancellationToken);
                    if (result != null)
                        return result;
                }

                _logger.LogWarning("Chat AI endpoint returned non-success status code: {StatusCode}", response.StatusCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while calling Chat AI endpoint.");
            }

            // Fallback response if HTTP call fails or service is unavailable
            return new ConversationApiResponseDto
            {
                Answer = "I am currently having trouble connecting to the knowledge base. Please try asking your question again in a moment.",
                RetrievedChunks = 0
            };
        }
    }
}
