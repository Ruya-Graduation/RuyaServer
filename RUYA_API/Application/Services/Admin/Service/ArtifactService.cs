using Microsoft.EntityFrameworkCore;
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

        public ArtifactService(RuyaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ArtifactDto>> GetAllAsync()
        {
            var artifacts = await _context.Artifacts
                .AsNoTracking()
                .ToListAsync();

            return artifacts.Select(a => a.ToDto());
        }

        public async Task<ArtifactDto?> GetByIdAsync(int id)
        {
            var artifact = await _context.Artifacts
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);

            if (artifact is null)
                throw new AppException($"Artifact with Id {id} was not found.", StatusCodes.Status404NotFound);

            return artifact.ToDto();
        }

        public async Task<ArtifactDto> CreateAsync(CreateArtifactDto dto)
        {
            await ValidateArtifact(dto);

            var artifact = dto.ToEntity();

            _context.Artifacts.Add(artifact);

            await _context.SaveChangesAsync();

            return artifact.ToDto();
        }

        public async Task UpdateAsync(int id, UpdateArtifactDto dto)
        {
            await ValidateArtifact(dto);

            var artifact = await _context.Artifacts
                .FirstOrDefaultAsync(a => a.Id == id);

            if (artifact is null)
                throw new AppException($"Artifact with Id {id} was not found.", StatusCodes.Status404NotFound);

            dto.UpdateEntity(artifact);

            await _context.SaveChangesAsync();
        }

        private async Task ValidateArtifact(CreateArtifactDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new AppException("Artifact name is required.", StatusCodes.Status400BadRequest);

            if (!await _context.Sites.AnyAsync(s => s.Id == dto.SiteId))
                throw new AppException("The selected site does not exist.", StatusCodes.Status400BadRequest);
        }

        private async Task ValidateArtifact(UpdateArtifactDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new AppException("Artifact name is required.", StatusCodes.Status400BadRequest);

            if (!await _context.Sites.AnyAsync(s => s.Id == dto.SiteId))
                throw new AppException("The selected site does not exist.", StatusCodes.Status400BadRequest);
        }
    }
}
