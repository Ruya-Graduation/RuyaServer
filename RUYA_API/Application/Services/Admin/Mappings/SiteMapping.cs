using RUYA_API.Application.Services.Admin.DTOs.Site;
using RUYA_API.Domain.Entities;

namespace RUYA_API.Application.Services.Admin.Mappings
{
    public static class SiteMapping
    {
        public static SiteDto ToDto(this Site site, string languageCode)
        {
            var translation = site.Translations.FirstOrDefault(t => t.LanguageCode == languageCode)
                            ?? site.Translations.FirstOrDefault(t => t.LanguageCode == "en");

            if (translation == null)
            {
                throw new InvalidOperationException($"No translation found for site {site.Id}");
            }

            return new SiteDto
            {
                Id = site.Id,
                Latitude = site.Latitude,
                Longitude = site.Longitude,
                ImageUrl = site.ImageUrl,
                Name = translation.Name,
                City = translation.City,
                Country = translation.Country,
                Hours = translation.Hours,
                Ticket = translation.Ticket,
                Crowds = translation.Crowds,
                Description = translation.Description
            };
        }

        public static Site ToEntity(this CreateSiteDto dto)
        {
            var site = new Site
            {
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                ImageUrl = string.Empty,
                ImagePublicId = string.Empty
            };

            foreach (var translationDto in dto.Translations)
            {
                site.Translations.Add(new SiteTranslation
                {
                    LanguageCode = translationDto.LanguageCode,
                    Name = translationDto.Name,
                    City = translationDto.City,
                    Country = translationDto.Country,
                    Hours = translationDto.Hours,
                    Ticket = translationDto.Ticket,
                    Crowds = translationDto.Crowds,
                    Description = translationDto.Description
                });
            }

            return site;
        }

        public static void UpdateEntity(this UpdateSiteDto dto, Site site)
        {
            site.Latitude = dto.Latitude;
            site.Longitude = dto.Longitude;

            // Remove existing translations
            site.Translations.Clear();

            // Add new translations
            foreach (var translationDto in dto.Translations)
            {
                site.Translations.Add(new SiteTranslation
                {
                    SiteId = site.Id,
                    LanguageCode = translationDto.LanguageCode,
                    Name = translationDto.Name,
                    City = translationDto.City,
                    Country = translationDto.Country,
                    Hours = translationDto.Hours,
                    Ticket = translationDto.Ticket,
                    Crowds = translationDto.Crowds,
                    Description = translationDto.Description
                });
            }
        }
    }
}
