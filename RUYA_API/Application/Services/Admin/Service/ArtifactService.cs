using Microsoft.EntityFrameworkCore;
using RUYA_API.Application.Common.Interfaces;
using RUYA_API.Application.Services.Admin.DTOs.Artifact;
using RUYA_API.Application.Services.Admin.Interfaces;
using RUYA_API.Application.Services.Admin.Mappings;
using RUYA_API.ExceptionHandling.CustomException;
using RUYA_API.Infrastructure.Context;

namespace RUYA_API.Application.Services.Admin.Service
{
    public class ArtifactService : IArtifactService
    {
        private readonly RuyaContext _context;
        private readonly IImageService _imageService;

        public ArtifactService(
            RuyaContext context,
            IImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        public async Task<IEnumerable<ArtifactDto>> GetAllAsync(string languageCode)
        {
            ValidateLanguageCode(languageCode);

            var artifacts = await _context.Artifacts
                .Include(a => a.Translations)
                .AsNoTracking()
                .ToListAsync();

            return artifacts.Select(a => a.ToDto(languageCode));
        }

        public async Task<ArtifactDto?> GetByIdAsync(int id, string languageCode)
        {
            ValidateLanguageCode(languageCode);

            var artifact = await _context.Artifacts
                .Include(a => a.Translations)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);

            if (artifact is null)
                throw new AppException($"Artifact with Id {id} was not found.", StatusCodes.Status404NotFound);

            return artifact.ToDto(languageCode);
        }

        public async Task<ArtifactDto> CreateAsync(CreateArtifactDto dto)
        {
            await ValidateArtifact(dto);
            ValidateTranslations(dto.Translations);

            var artifact = dto.ToEntity();

            if (dto.Image is not null)
            {
                var uploadResult = await _imageService.UploadImageAsync(dto.Image);

                artifact.ImageUrl = uploadResult.ImageUrl;
                artifact.ImagePublicId = uploadResult.PublicId;
            }

            _context.Artifacts.Add(artifact);

            await _context.SaveChangesAsync();

            // Return with English translation by default
            var createdArtifact = await _context.Artifacts
                .Include(a => a.Translations)
                .FirstAsync(a => a.Id == artifact.Id);

            return createdArtifact.ToDto("en");
        }

        public async Task UpdateAsync(int id, UpdateArtifactDto dto)
        {
            await ValidateArtifact(dto);
            ValidateTranslations(dto.Translations);

            var artifact = await _context.Artifacts
                .Include(a => a.Translations)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (artifact is null)
                throw new AppException($"Artifact with Id {id} was not found.", StatusCodes.Status404NotFound);

            dto.UpdateEntity(artifact);

            if (dto.Image is not null)
            {
                if (!string.IsNullOrWhiteSpace(artifact.ImagePublicId))
                {
                    await _imageService.DeleteImageAsync(artifact.ImagePublicId);
                }

                var uploadResult = await _imageService.UploadImageAsync(dto.Image);

                artifact.ImageUrl = uploadResult.ImageUrl;
                artifact.ImagePublicId = uploadResult.PublicId;
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var artifact = await _context.Artifacts.FindAsync(id);

            if (artifact is null)
            {
                throw new AppException($"Artifact with Id {id} was not found.", StatusCodes.Status404NotFound);
            }

            try
            {
                if (!string.IsNullOrWhiteSpace(artifact.ImagePublicId))
                {
                    await _imageService.DeleteImageAsync(artifact.ImagePublicId);
                }

                _context.Artifacts.Remove(artifact);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new AppException("This artifact cannot be deleted because it is referenced by other records.", StatusCodes.Status400BadRequest);
            }
        }

        private async Task ValidateArtifact(CreateArtifactDto dto)
        {
            if (!await _context.Sites.AnyAsync(s => s.Id == dto.SiteId))
                throw new AppException("The selected site does not exist.", StatusCodes.Status400BadRequest);
        }

        private async Task ValidateArtifact(UpdateArtifactDto dto)
        {
            if (!await _context.Sites.AnyAsync(s => s.Id == dto.SiteId))
                throw new AppException("The selected site does not exist.", StatusCodes.Status400BadRequest);
        }

        private static void ValidateLanguageCode(string languageCode)
        {
            if (languageCode != "ar" && languageCode != "en")
                throw new AppException("Language code must be 'ar' or 'en'.", StatusCodes.Status400BadRequest);
        }

        private static void ValidateTranslations(List<ArtifactTranslationDto> translations)
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
                    throw new AppException($"Artifact name is required for language '{translation.LanguageCode}'.", StatusCodes.Status400BadRequest);
            }
        }
    }
}
