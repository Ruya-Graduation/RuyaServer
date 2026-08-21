namespace RUYA_API.Application.Services.Moments.DTOs
{
    public class MomentAlbumDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string StartDate { get; set; } = string.Empty;
        public string? CoverPhotoUrl { get; set; }
        public int PhotoCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<MomentPhotoDto> Photos { get; set; } = new();
    }
}
