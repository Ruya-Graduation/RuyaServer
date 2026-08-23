using RUYA_API.Application.Services.Moments.DTOs;

namespace RUYA_API.Application.Services.Moments.Interfaces
{
    public interface IMomentsService
    {
        Task<IEnumerable<MomentAlbumDto>> GetAlbumsAsync(string userId);
        Task<MomentAlbumDetailsDto> GetAlbumByIdAsync(int albumId, string userId);
        Task<MomentAlbumDto> CreateAlbumAsync(CreateMomentAlbumDto dto, string userId);
        Task<MomentAlbumDetailsDto> AddPhotoAsync(int albumId, AddPhotoToAlbumDto dto, string userId);
        Task DeletePhotoAsync(int albumId, int photoId, string userId);
        Task<MomentAlbumDto> UpdateAlbumAsync(int albumId, UpdateMomentAlbumDto dto, string userId);
        Task DeleteAlbumAsync(int albumId, string userId);
    }
}
