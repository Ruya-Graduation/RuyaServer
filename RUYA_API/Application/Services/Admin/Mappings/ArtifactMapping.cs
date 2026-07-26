using RUYA_API.Application.Services.Admin.DTOs.Artifact;
using RUYA_API.Domain.Entities;

namespace RUYA_API.Application.Services.Admin.Mappings
{
    public static class ArtifactMapping
    {
        public static ArtifactDto ToDto(this Artifact artifact)
        {
            return new ArtifactDto
            {
                Id = artifact.Id,
                SiteId = artifact.SiteId,
                Name = artifact.Name,
                Category = artifact.Category,
                Civilization = artifact.Civilization,
                Period = artifact.Period,
                ImageUrl = artifact.ImageUrl
            };
        }

        public static Artifact ToEntity(this CreateArtifactDto dto)
        {
            return new Artifact
            {
                SiteId = dto.SiteId,
                Name = dto.Name,
                Category = dto.Category,
                Civilization = dto.Civilization,
                Period = dto.Period,
                ImageUrl = dto.ImageUrl
            };
        }

        public static void UpdateEntity(this UpdateArtifactDto dto, Artifact artifact)
        {
            artifact.SiteId = dto.SiteId;
            artifact.Name = dto.Name;
            artifact.Category = dto.Category;
            artifact.Civilization = dto.Civilization;
            artifact.Period = dto.Period;
            artifact.ImageUrl = dto.ImageUrl;
        }
    }
}
