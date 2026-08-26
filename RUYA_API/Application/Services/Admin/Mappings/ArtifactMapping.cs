using RUYA_API.Application.Services.Admin.DTOs.Artifact;
using RUYA_API.Domain.Entities;

namespace RUYA_API.Application.Services.Admin.Mappings
{
    public static class ArtifactMapping
    {
        public static ArtifactDto ToDto(this Artifact artifact, string languageCode)
        {
            var translation = artifact.Translations.FirstOrDefault(t => t.LanguageCode == languageCode)
                            ?? artifact.Translations.FirstOrDefault(t => t.LanguageCode == "en");

            if (translation == null)
            {
                throw new InvalidOperationException($"No translation found for artifact {artifact.Id}");
            }

            return new ArtifactDto
            {
                Id = artifact.Id,
                SiteId = artifact.SiteId,
                ImageUrl = artifact.ImageUrl,
                Name = translation.Name,
                Category = translation.Category,
                Civilization = translation.Civilization,
                Period = translation.Period,
                Material = translation.Material,
                PlaceOfDiscovery = translation.PlaceOfDiscovery
            };
        }

        public static Artifact ToEntity(this CreateArtifactDto dto)
        {
            var artifact = new Artifact
            {
                SiteId = dto.SiteId
            };

            foreach (var translationDto in dto.Translations)
            {
                artifact.Translations.Add(new ArtifactTranslation
                {
                    LanguageCode = translationDto.LanguageCode,
                    Name = translationDto.Name,
                    Category = translationDto.Category,
                    Civilization = translationDto.Civilization,
                    Period = translationDto.Period,
                    Material = translationDto.Material,
                    PlaceOfDiscovery = translationDto.PlaceOfDiscovery
                });
            }

            return artifact;
        }

        public static void UpdateEntity(this UpdateArtifactDto dto, Artifact artifact)
        {
            artifact.SiteId = dto.SiteId;

            // Remove existing translations
            artifact.Translations.Clear();

            // Add new translations
            foreach (var translationDto in dto.Translations)
            {
                artifact.Translations.Add(new ArtifactTranslation
                {
                    ArtifactId = artifact.Id,
                    LanguageCode = translationDto.LanguageCode,
                    Name = translationDto.Name,
                    Category = translationDto.Category,
                    Civilization = translationDto.Civilization,
                    Period = translationDto.Period,
                    Material = translationDto.Material,
                    PlaceOfDiscovery = translationDto.PlaceOfDiscovery
                });
            }
        }
    }
}
