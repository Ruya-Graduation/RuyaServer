namespace RUYA_API.Application.Services.Chat.DTOs
{
    /// <summary>
    /// Response from POST /conversation endpoint in Python AI service
    /// </summary>
    public class ConversationApiResponseDto
    {
        public string Answer { get; set; } = string.Empty;
        public int RetrievedChunks { get; set; }
        public string? ModelId { get; set; }
        public List<RetrievedChunkDto>? Sources { get; set; }
        public string? RequestId { get; set; }
        public string? Region { get; set; }
        public UsageDto? Usage { get; set; }
        public double? EstimatedCostUsd { get; set; }
        public double? ActualCostUsd { get; set; }
        public string? Status { get; set; }
    }

    public class RetrievedChunkDto
    {
        public string? ChunkId { get; set; }
        public string? Title { get; set; }
        public string? Text { get; set; }
        public int? PageStart { get; set; }
        public int? PageEnd { get; set; }
        public double? Score { get; set; }
    }

    public class UsageDto
    {
        public int? PromptTokens { get; set; }
        public int? CompletionTokens { get; set; }
        public int? TotalTokens { get; set; }
    }
}
