using Microsoft.AspNetCore.Mvc;
using RUYA_API.Application.Services.Admin.DTOs.Artifact;
using RUYA_API.Application.Services.Admin.Interfaces;
using RUYA_API.Responses;

namespace RUYA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminArtifactsController : ControllerBase
    {
        private readonly IArtifactService _artifactService;

        public AdminArtifactsController(IArtifactService artifactService)
        {
            _artifactService = artifactService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var artifacts = await _artifactService.GetAllAsync();
            return Ok(ResponseFactory.Success(artifacts, "Artifacts retrieved successfully."));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var artifact = await _artifactService.GetByIdAsync(id);
            return Ok(ResponseFactory.Success(artifact, "Artifact retrieved successfully."));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateArtifactDto dto)
        {
            var artifact = await _artifactService.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById),
                new { id = artifact.Id },
                ResponseFactory.Success(artifact, "Artifact created successfully."));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateArtifactDto dto)
        {
            await _artifactService.UpdateAsync(id, dto);
            return Ok(ResponseFactory.Success(message: "Artifact updated successfully."));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _artifactService.DeleteAsync(id);

            return Ok(ResponseFactory.Success("Artifact deleted successfully."));
        }
    }
}
