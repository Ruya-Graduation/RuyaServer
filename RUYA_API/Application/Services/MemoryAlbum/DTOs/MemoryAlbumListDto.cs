namespace RUYA_API.Application.Services.MemoryAlbum.DTOs
{
    public class MemoryAlbumListDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Year { get; set; }
        public string CoverImage { get; set; } = string.Empty;
    }
}
