namespace RUYA_API.Application.Services.Moments.DTOs
{
    public class MomentPhotoDto
    {
        public int Id { get; set; }
        public string PhotoUrl { get; set; } = string.Empty;
        public string? Caption { get; set; }
        public string? DayLabel { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
