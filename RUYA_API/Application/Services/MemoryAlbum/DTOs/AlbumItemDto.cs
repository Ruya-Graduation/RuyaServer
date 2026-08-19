namespace RUYA_API.Application.Services.MemoryAlbum.DTOs
{
    public class AlbumItemDto
    {
        public int Id { get; set; }
        public int ArtifactId { get; set; }
        public string PhotoUrl { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
    }
}
