using RUYA_API.Application.Services.Admin.DTOs.Site;
using RUYA_API.Domain.Entities;

namespace RUYA_API.Application.Services.Admin.Mappings
{
    public static class SiteMapping
    {
        public static SiteDto ToDto(this Site site)
        {
            return new SiteDto
            {
                Id = site.Id,
                Name = site.Name,
                City = site.City,
                Country = site.Country,
                Latitude = site.Latitude,
                Longitude = site.Longitude,
                Hours = site.Hours,
                Ticket = site.Ticket,
                Crowds = site.Crowds,
                Description = site.Description
            };
        }

        public static Site ToEntity(this CreateSiteDto dto)
        {
            return new Site
            {
                Name = dto.Name,
                City = dto.City,
                Country = dto.Country,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Hours = dto.Hours,
                Ticket = dto.Ticket,
                Crowds = dto.Crowds,
                Description = dto.Description
            };
        }

        public static void UpdateEntity(this UpdateSiteDto dto, Site site)
        {
            site.Name = dto.Name;
            site.City = dto.City;
            site.Country = dto.Country;
            site.Latitude = dto.Latitude;
            site.Longitude = dto.Longitude;
            site.Hours = dto.Hours;
            site.Ticket = dto.Ticket;
            site.Crowds = dto.Crowds;
            site.Description = dto.Description;
        }
    }
}
