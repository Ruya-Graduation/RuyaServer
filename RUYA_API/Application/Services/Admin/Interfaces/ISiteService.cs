using RUYA_API.Application.Services.Admin.DTOs.Site;

namespace RUYA_API.Application.Services.Admin.Interfaces
{
    public interface ISiteService
    {
        Task<IEnumerable<SiteDto>> GetAllAsync();

        Task<SiteDto?> GetByIdAsync(int id);

        Task<SiteDto> CreateAsync(CreateSiteDto dto);

        Task UpdateAsync(int id, UpdateSiteDto dto);
    }
}
