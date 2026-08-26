using RUYA_API.Application.Services.Admin.DTOs.Artifact;

namespace RUYA_API.Application.Services.Admin.Interfaces
{
    public interface IArtifactService
    {
        Task<IEnumerable<ArtifactDto>> GetAllAsync(string languageCode);

        Task<ArtifactDto?> GetByIdAsync(int id, string languageCode);

        Task<ArtifactDto> CreateAsync(CreateArtifactDto dto);

        Task UpdateAsync(int id, UpdateArtifactDto dto);

        Task DeleteAsync(int id);
    }
}
