using Microsoft.EntityFrameworkCore;
using RUYA_API.Application.Common.Interfaces;
using RUYA_API.Application.Services.Admin.DTOs.Site;
using RUYA_API.Application.Services.Admin.Interfaces;
using RUYA_API.Application.Services.Admin.Mappings;
using RUYA_API.ExceptionHandling.CustomException;
using RUYA_API.Infrastructure.Context;

namespace RUYA_API.Application.Services.Admin.Service
{
    public class SiteService : ISiteService
    {
        private readonly RuyaContext _context;
        private readonly IImageService _imageService;

        public SiteService(
            RuyaContext context,
            IImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        public async Task<IEnumerable<SiteDto>> GetAllAsync()
        {
            var sites = await _context.Sites
                .AsNoTracking()
                .ToListAsync();

            return sites.Select(s => s.ToDto());
        }

        public async Task<SiteDto?> GetByIdAsync(int id)
        {
            var site = await _context.Sites
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);

            if (site is null)
                throw new AppException($"Site with Id {id} was not found.", StatusCodes.Status404NotFound);

            return site.ToDto();
        }

        public async Task<SiteDto> CreateAsync(CreateSiteDto dto)
        {
            ValidateSite(dto.Name, dto.Latitude, dto.Longitude);

            var site = dto.ToEntity();

            if (dto.Image is not null)
            {
                var uploadResult = await _imageService.UploadImageAsync(dto.Image);

                site.ImageUrl = uploadResult.ImageUrl;
                site.ImagePublicId = uploadResult.PublicId;
            }

            _context.Sites.Add(site);

            await _context.SaveChangesAsync();

            return site.ToDto();
        }

        public async Task UpdateAsync(int id, UpdateSiteDto dto)
        {
            ValidateSite(dto.Name, dto.Latitude, dto.Longitude);

            var site = await _context.Sites
                .FirstOrDefaultAsync(s => s.Id == id);

            if (site is null)
                throw new AppException($"Site with Id {id} was not found.", StatusCodes.Status404NotFound);

            dto.UpdateEntity(site);

            if (dto.Image is not null)
            {
                if (!string.IsNullOrWhiteSpace(site.ImagePublicId))
                {
                    await _imageService.DeleteImageAsync(site.ImagePublicId);
                }

                var uploadResult = await _imageService.UploadImageAsync(dto.Image);

                site.ImageUrl = uploadResult.ImageUrl;
                site.ImagePublicId = uploadResult.PublicId;
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var site = await _context.Sites.FindAsync(id);

            if (site is null)
            {
                throw new AppException($"Site with Id {id} was not found.", StatusCodes.Status404NotFound);
            }

            try
            {
                if (!string.IsNullOrWhiteSpace(site.ImagePublicId))
                {
                    await _imageService.DeleteImageAsync(site.ImagePublicId);
                }

                _context.Sites.Remove(site);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new AppException("This site cannot be deleted because it is referenced by other records.", StatusCodes.Status400BadRequest);
            }
        }

        private static void ValidateSite(string name, float latitude, float longitude)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new AppException("Site name is required.", StatusCodes.Status400BadRequest);

            if (latitude < -90 || latitude > 90)
                throw new AppException("Latitude must be between -90 and 90.", StatusCodes.Status400BadRequest);

            if (longitude < -180 || longitude > 180)
                throw new AppException("Longitude must be between -180 and 180.", StatusCodes.Status400BadRequest);
        }
    }
}
