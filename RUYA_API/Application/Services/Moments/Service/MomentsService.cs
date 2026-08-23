using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RUYA_API.Application.Common.Interfaces;
using RUYA_API.Application.Services.Moments.DTOs;
using RUYA_API.Application.Services.Moments.Interfaces;
using RUYA_API.Domain.Entities;
using RUYA_API.ExceptionHandling.CustomException;
using RUYA_API.Infrastructure.Context;

namespace RUYA_API.Application.Services.Moments.Service
{
    public class MomentsService : IMomentsService
    {
        private readonly RuyaContext _context;
        private readonly IImageService _imageService;
        private readonly ILogger<MomentsService> _logger;

        public MomentsService(
            RuyaContext context,
            IImageService imageService,
            ILogger<MomentsService> logger)
        {
            _context = context;
            _imageService = imageService;
            _logger = logger;
        }

        public async Task<IEnumerable<MomentAlbumDto>> GetAlbumsAsync(string userId)
        {
            var albums = await _context.MemoryAlbums
                .Include(a => a.AlbumItems)
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.CreatedAt)
                .AsNoTracking()
                .ToListAsync();

            return albums.Select(a => new MomentAlbumDto
            {
                Id = a.Id,
                Title = a.Title,
                StartDate = a.StartDate,
                CoverPhotoUrl = a.AlbumItems.FirstOrDefault()?.PhotoUrl,
                PhotoCount = a.AlbumItems.Count,
                CreatedAt = a.CreatedAt
            });
        }

        public async Task<MomentAlbumDetailsDto> GetAlbumByIdAsync(int albumId, string userId)
        {
            var album = await _context.MemoryAlbums
                .Include(a => a.AlbumItems)
                .Where(a => a.Id == albumId)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (album is null)
            {
                throw new AppException(
                    "Album not found.",
                    StatusCodes.Status404NotFound);
            }

            if (album.UserId != userId)
            {
                throw new AppException(
                    "You are not allowed to access this album.",
                    StatusCodes.Status403Forbidden);
            }

            return new MomentAlbumDetailsDto
            {
                Id = album.Id,
                Title = album.Title,
                StartDate = album.StartDate,
                CoverPhotoUrl = album.AlbumItems.FirstOrDefault()?.PhotoUrl,
                PhotoCount = album.AlbumItems.Count,
                CreatedAt = album.CreatedAt,
                Photos = album.AlbumItems
                    .OrderBy(p => p.CreatedAt)
                    .Select(p => new MomentPhotoDto
                    {
                        Id = p.Id,
                        PhotoUrl = p.PhotoUrl,
                        Caption = p.Caption,
                        DayLabel = p.DayLabel,
                        CreatedAt = p.CreatedAt
                    })
                    .ToList()
            };
        }

        public async Task<MomentAlbumDto> CreateAlbumAsync(CreateMomentAlbumDto dto, string userId)
        {
            _logger.LogInformation("Creating album for user {UserId}", userId);

            var album = new Domain.Entities.MemoryAlbum
            {
                UserId = userId,
                Title = dto.Title,
                StartDate = dto.StartDate,
                TourId = null,
                CoverImage = string.Empty,
                SummaryText = null
            };

            _context.MemoryAlbums.Add(album);

            // If cover photo provided, upload and create first album item
            if (dto.CoverPhoto != null)
            {
                _logger.LogInformation("Uploading cover photo for album");

                var (imageUrl, publicId) = await _imageService.UploadImageAsync(dto.CoverPhoto);

                var coverItem = new AlbumItem
                {
                    AlbumId = album.Id,
                    PhotoUrl = imageUrl,
                    PublicId = publicId,
                    Caption = "Cover",
                    DayLabel = null,
                    ArtifactId = null,
                    AiSummary = null
                };

                album.AlbumItems.Add(coverItem);
                album.CoverImage = imageUrl;
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation("Album created with ID {AlbumId}", album.Id);

            return new MomentAlbumDto
            {
                Id = album.Id,
                Title = album.Title,
                StartDate = album.StartDate,
                CoverPhotoUrl = album.AlbumItems.FirstOrDefault()?.PhotoUrl,
                PhotoCount = album.AlbumItems.Count,
                CreatedAt = album.CreatedAt
            };
        }

        public async Task<MomentAlbumDetailsDto> AddPhotoAsync(int albumId, AddPhotoToAlbumDto dto, string userId)
        {
            var album = await _context.MemoryAlbums
                .Include(a => a.AlbumItems)
                .FirstOrDefaultAsync(a => a.Id == albumId);

            if (album is null)
            {
                throw new AppException(
                    "Album not found.",
                    StatusCodes.Status404NotFound);
            }

            if (album.UserId != userId)
            {
                throw new AppException(
                    "You are not allowed to access this album.",
                    StatusCodes.Status403Forbidden);
            }

            _logger.LogInformation("Uploading photo to album {AlbumId}", albumId);

            var (imageUrl, publicId) = await _imageService.UploadImageAsync(dto.Photo);

            var albumItem = new AlbumItem
            {
                AlbumId = albumId,
                PhotoUrl = imageUrl,
                PublicId = publicId,
                Caption = dto.Caption,
                DayLabel = dto.DayLabel,
                ArtifactId = null,
                AiSummary = null
            };

            _context.AlbumItems.Add(albumItem);

            // Update cover image if this is the first photo
            if (!album.AlbumItems.Any())
            {
                album.CoverImage = imageUrl;
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation("Photo added to album {AlbumId}", albumId);

            // Return updated album details
            return await GetAlbumByIdAsync(albumId, userId);
        }

        public async Task DeletePhotoAsync(int albumId, int photoId, string userId)
        {
            var album = await _context.MemoryAlbums
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == albumId);

            if (album is null)
            {
                throw new AppException(
                    "Album not found.",
                    StatusCodes.Status404NotFound);
            }

            if (album.UserId != userId)
            {
                throw new AppException(
                    "You are not allowed to access this album.",
                    StatusCodes.Status403Forbidden);
            }

            var albumItem = await _context.AlbumItems
                .FirstOrDefaultAsync(ai => ai.Id == photoId && ai.AlbumId == albumId);

            if (albumItem is null)
            {
                throw new AppException(
                    "Photo not found.",
                    StatusCodes.Status404NotFound);
            }

            _logger.LogInformation("Deleting photo {PhotoId} from album {AlbumId}", photoId, albumId);

            // Delete from Cloudinary if PublicId is set
            if (!string.IsNullOrEmpty(albumItem.PublicId))
            {
                try
                {
                    await _imageService.DeleteImageAsync(albumItem.PublicId);
                    _logger.LogInformation("Deleted image from Cloudinary: {PublicId}", albumItem.PublicId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to delete image from Cloudinary: {PublicId}", albumItem.PublicId);
                    // Continue with database deletion even if Cloudinary deletion fails
                }
            }

            _context.AlbumItems.Remove(albumItem);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Photo deleted from album {AlbumId}", albumId);
        }

        public async Task<MomentAlbumDto> UpdateAlbumAsync(int albumId, UpdateMomentAlbumDto dto, string userId)
        {
            var album = await _context.MemoryAlbums
                .Include(a => a.AlbumItems)
                .FirstOrDefaultAsync(a => a.Id == albumId);

            if (album is null)
            {
                throw new AppException(
                    "Album not found.",
                    StatusCodes.Status404NotFound);
            }

            if (album.UserId != userId)
            {
                throw new AppException(
                    "You are not allowed to access this album.",
                    StatusCodes.Status403Forbidden);
            }

            _logger.LogInformation("Updating album {AlbumId}", albumId);

            // Update only the provided fields
            if (!string.IsNullOrEmpty(dto.Title))
            {
                album.Title = dto.Title;
            }

            if (!string.IsNullOrEmpty(dto.StartDate))
            {
                album.StartDate = dto.StartDate;
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation("Album {AlbumId} updated", albumId);

            return new MomentAlbumDto
            {
                Id = album.Id,
                Title = album.Title,
                StartDate = album.StartDate,
                CoverPhotoUrl = album.AlbumItems.FirstOrDefault()?.PhotoUrl,
                PhotoCount = album.AlbumItems.Count,
                CreatedAt = album.CreatedAt
            };
        }

        public async Task DeleteAlbumAsync(int albumId, string userId)
        {
            var album = await _context.MemoryAlbums
                .Include(a => a.AlbumItems)
                .FirstOrDefaultAsync(a => a.Id == albumId);

            if (album is null)
            {
                throw new AppException(
                    "Album not found.",
                    StatusCodes.Status404NotFound);
            }

            if (album.UserId != userId)
            {
                throw new AppException(
                    "You are not allowed to access this album.",
                    StatusCodes.Status403Forbidden);
            }

            _logger.LogInformation("Deleting album {AlbumId}", albumId);

            // Delete all photos from Cloudinary
            foreach (var item in album.AlbumItems)
            {
                if (!string.IsNullOrEmpty(item.PublicId))
                {
                    try
                    {
                        await _imageService.DeleteImageAsync(item.PublicId);
                        _logger.LogInformation("Deleted image from Cloudinary: {PublicId}", item.PublicId);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to delete image from Cloudinary: {PublicId}", item.PublicId);
                        // Continue with next image
                    }
                }
            }

            _context.MemoryAlbums.Remove(album);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Album {AlbumId} deleted", albumId);
        }
    }
}
