using Microsoft.EntityFrameworkCore;
using RUYA_API.Application.Services.MemoryAlbum.DTOs;
using RUYA_API.Application.Services.MemoryAlbum.Interfaces;
using RUYA_API.Application.Services.MemoryAlbum.Mappings;
using RUYA_API.ExceptionHandling.CustomException;
using RUYA_API.Infrastructure.Context;

namespace RUYA_API.Application.Services.MemoryAlbum.Service
{
    public class MemoryAlbumService : IMemoryAlbumService
    {
        private readonly RuyaContext _context;

        public MemoryAlbumService(RuyaContext context)
        {
            _context = context;
        }

        public async Task<MemoryAlbumDto> CreateAsync(CreateMemoryAlbumDto dto, string userId)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new AppException("Memory Album title is required.", StatusCodes.Status400BadRequest);

            if (dto.Year < 1900 || dto.Year > 2100)
                throw new AppException("Year must be between 1900 and 2100.", StatusCodes.Status400BadRequest);

            var album = new Domain.Entities.MemoryAlbum
            {
                Title = dto.Title,
                Year = dto.Year,
                CoverImage = dto.CoverImage,
                UserId = userId,
                GeneratedAt = DateTime.UtcNow,
                SummaryText = string.Empty,
                TourId = null // Not associated with any tour
            };

            foreach (var itemDto in dto.AlbumItems)
            {
                var artifactExists = await _context.Artifacts.AnyAsync(a => a.Id == itemDto.ArtifactId);
                if (!artifactExists)
                    throw new AppException($"Artifact with Id {itemDto.ArtifactId} was not found.", StatusCodes.Status404NotFound);

                album.AlbumItems.Add(new Domain.Entities.AlbumItem
                {
                    ArtifactId = itemDto.ArtifactId,
                    PhotoUrl = itemDto.PhotoUrl,
                    Label = itemDto.Label,
                    AiSummary = string.Empty
                });
            }

            _context.MemoryAlbums.Add(album);
            await _context.SaveChangesAsync();

            return album.ToDto();
        }

        public async Task UpdateAsync(int id, UpdateMemoryAlbumDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new AppException("Memory Album title is required.", StatusCodes.Status400BadRequest);

            if (dto.Year < 1900 || dto.Year > 2100)
                throw new AppException("Year must be between 1900 and 2100.", StatusCodes.Status400BadRequest);

            var album = await _context.MemoryAlbums
                .FirstOrDefaultAsync(a => a.Id == id);

            if (album is null)
                throw new AppException($"Memory Album with Id {id} was not found.", StatusCodes.Status404NotFound);

            dto.UpdateEntity(album);

            await _context.SaveChangesAsync();
        }

        public async Task<MemoryAlbumDto?> GetByIdAsync(int id)
        {
            var album = await _context.MemoryAlbums
                .Include(a => a.AlbumItems)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);

            if (album is null)
                throw new AppException($"Memory Album with Id {id} was not found.", StatusCodes.Status404NotFound);

            return album.ToDto();
        }

        public async Task<IEnumerable<MemoryAlbumListDto>> GetAllAsync()
        {
            var albums = await _context.MemoryAlbums
                .AsNoTracking()
                .ToListAsync();

            return albums.Select(a => a.ToListDto());
        }

        public async Task<AlbumItemDto> AddAlbumItemAsync(int albumId, AddAlbumItemDto dto)
        {
            var album = await _context.MemoryAlbums
                .FirstOrDefaultAsync(a => a.Id == albumId);

            if (album is null)
                throw new AppException($"Memory Album with Id {albumId} was not found.", StatusCodes.Status404NotFound);

            var artifactExists = await _context.Artifacts.AnyAsync(a => a.Id == dto.ArtifactId);
            if (!artifactExists)
                throw new AppException($"Artifact with Id {dto.ArtifactId} was not found.", StatusCodes.Status404NotFound);

            var albumItem = new Domain.Entities.AlbumItem
            {
                AlbumId = albumId,
                ArtifactId = dto.ArtifactId,
                PhotoUrl = dto.PhotoUrl,
                Label = dto.Label,
                AiSummary = string.Empty
            };

            _context.AlbumItems.Add(albumItem);
            await _context.SaveChangesAsync();

            return albumItem.ToDto();
        }

        public async Task DeleteAlbumItemAsync(int albumId, int itemId)
        {
            var album = await _context.MemoryAlbums
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == albumId);

            if (album is null)
                throw new AppException($"Memory Album with Id {albumId} was not found.", StatusCodes.Status404NotFound);

            var albumItem = await _context.AlbumItems
                .FirstOrDefaultAsync(ai => ai.Id == itemId && ai.AlbumId == albumId);

            if (albumItem is null)
                throw new AppException($"Album Item with Id {itemId} was not found in Memory Album {albumId}.", StatusCodes.Status404NotFound);

            _context.AlbumItems.Remove(albumItem);
            await _context.SaveChangesAsync();
        }
    }
}
