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

        public async Task<IEnumerable<SiteDto>> GetAllAsync(string languageCode)
        {
            ValidateLanguageCode(languageCode);

            var sites = await _context.Sites
                .Include(s => s.Translations)
                .AsNoTracking()
                .ToListAsync();

            return sites.Select(s => s.ToDto(languageCode));
        }

        public async Task<SiteDto?> GetByIdAsync(int id, string languageCode)
        {
            ValidateLanguageCode(languageCode);

            var site = await _context.Sites
                .Include(s => s.Translations)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);

            if (site is null)
                throw new AppException($"Site with Id {id} was not found.", StatusCodes.Status404NotFound);

            return site.ToDto(languageCode);
        }

        public async Task<SiteDto> CreateAsync(CreateSiteDto dto)
        {
            ValidateSite(dto.Latitude, dto.Longitude);
            ValidateTranslations(dto.Translations);

            var site = dto.ToEntity();

            if (dto.Image is not null)
            {
                var uploadResult = await _imageService.UploadImageAsync(dto.Image);

                site.ImageUrl = uploadResult.ImageUrl;
                site.ImagePublicId = uploadResult.PublicId;
            }

            _context.Sites.Add(site);

            await _context.SaveChangesAsync();

            // Return with English translation by default
            var createdSite = await _context.Sites
                .Include(s => s.Translations)
                .FirstAsync(s => s.Id == site.Id);

            return createdSite.ToDto("en");
        }

        public async Task UpdateAsync(int id, UpdateSiteDto dto)
        {
            ValidateSite(dto.Latitude, dto.Longitude);
            ValidateTranslations(dto.Translations);

            var site = await _context.Sites
                .Include(s => s.Translations)
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

        private static void ValidateSite(float latitude, float longitude)
        {
            if (latitude < -90 || latitude > 90)
                throw new AppException("Latitude must be between -90 and 90.", StatusCodes.Status400BadRequest);

            if (longitude < -180 || longitude > 180)
                throw new AppException("Longitude must be between -180 and 180.", StatusCodes.Status400BadRequest);
        }

        private static void ValidateLanguageCode(string languageCode)
        {
            if (languageCode != "ar" && languageCode != "en")
                throw new AppException("Language code must be 'ar' or 'en'.", StatusCodes.Status400BadRequest);
        }

        private static void ValidateTranslations(List<SiteTranslationDto> translations)
        {
            if (translations == null || !translations.Any())
                throw new AppException("At least one translation is required.", StatusCodes.Status400BadRequest);

            var languageCodes = translations.Select(t => t.LanguageCode).ToList();
            var duplicates = languageCodes.GroupBy(x => x).Where(g => g.Count() > 1).Select(g => g.Key).ToList();

            if (duplicates.Any())
                throw new AppException($"Duplicate translations for language(s): {string.Join(", ", duplicates)}", StatusCodes.Status400BadRequest);

            foreach (var translation in translations)
            {
                if (string.IsNullOrWhiteSpace(translation.Name))
                    throw new AppException($"Site name is required for language '{translation.LanguageCode}'.", StatusCodes.Status400BadRequest);
            }
        }
    }
}
