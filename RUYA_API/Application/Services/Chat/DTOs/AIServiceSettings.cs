namespace RUYA_API.Application.Services.Chat.DTOs
{
    public class AIServiceSettings
    {
        public string VisionBaseUrl { get; set; } = "https://predict-6a852998704ca7a1f66f31e3-dproatj77a-ww.a.run.app";
        public string ChatBaseUrl { get; set; } = "https://ruayconversationservice-production.up.railway.app";
        public string VisionEndpoint { get; set; } = "/predict";
        public string ChatEndpoint { get; set; } = "/conversation";
        public double ConfidenceThreshold { get; set; } = 0.25;
        public int HistoryWindowSize { get; set; } = 25;
        public string? ApiKey { get; set; }
        public double Conf { get; set; } = 0.25;
        public double Iou { get; set; } = 0.7;
        public int Imgsz { get; set; } = 640;
    }
}
