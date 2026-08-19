using RUYA_API.Application.Services.MemoryAlbum.DTOs;

namespace RUYA_API.Application.Services.MemoryAlbum.Mappings
{
    public static class MemoryAlbumMapping
    {
        public static MemoryAlbumDto ToDto(this Domain.Entities.MemoryAlbum album)
        {
            return new MemoryAlbumDto
            {
                Id = album.Id,
                Title = album.Title,
                Year = album.Year,
                CoverImage = album.CoverImage,
                AlbumItems = album.AlbumItems.Select(ai => ai.ToDto()).ToList()
            };
        }

        public static MemoryAlbumListDto ToListDto(this Domain.Entities.MemoryAlbum album)
        {
            return new MemoryAlbumListDto
            {
                Id = album.Id,
                Title = album.Title,
                Year = album.Year,
                CoverImage = album.CoverImage
            };
        }

        public static AlbumItemDto ToDto(this Domain.Entities.AlbumItem item)
        {
            return new AlbumItemDto
            {
                Id = item.Id,
                ArtifactId = item.ArtifactId,
                PhotoUrl = item.PhotoUrl,
                Label = item.Label
            };
        }

        public static void UpdateEntity(this UpdateMemoryAlbumDto dto, Domain.Entities.MemoryAlbum album)
        {
            album.Title = dto.Title;
            album.Year = dto.Year;
            album.CoverImage = dto.CoverImage;
        }
    }
}
