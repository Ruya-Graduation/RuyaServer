using RUYA_API.Application.Services.MemoryAlbum.DTOs;

namespace RUYA_API.Application.Services.MemoryAlbum.Interfaces
{
    public interface IMemoryAlbumService
    {
        Task<MemoryAlbumDto> CreateAsync(CreateMemoryAlbumDto dto, string userId);
        Task UpdateAsync(int id, UpdateMemoryAlbumDto dto);
        Task<MemoryAlbumDto?> GetByIdAsync(int id);
        Task<IEnumerable<MemoryAlbumListDto>> GetAllAsync();
        Task<AlbumItemDto> AddAlbumItemAsync(int albumId, AddAlbumItemDto dto);
        Task DeleteAlbumItemAsync(int albumId, int itemId);
    }
}
