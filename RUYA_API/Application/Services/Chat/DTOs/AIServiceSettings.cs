namespace RUYA_API.Application.Services.Chat.DTOs
{
    public class AIServiceSettings
    {
        public string BaseUrl { get; set; } = "http://localhost:8000";
        public string VisionEndpoint { get; set; } = "/api/v1/vision/recognize";
        public string ChatEndpoint { get; set; } = "/api/v1/chat/generate";
        public double ConfidenceThreshold { get; set; } = 0.75; // 75%
        public int HistoryWindowSize { get; set; } = 25;
    }
}
