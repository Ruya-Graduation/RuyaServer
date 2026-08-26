using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RUYA_API.Application.Services.Admin.DTOs.Site;
using RUYA_API.Application.Services.Admin.Interfaces;
using RUYA_API.Responses;

namespace RUYA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminSitesController : ControllerBase
    {
        private readonly ISiteService _siteService;

        public AdminSitesController(ISiteService siteService)
        {
            _siteService = siteService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string lang = "en")
        {
            var sites = await _siteService.GetAllAsync(lang);
            return Ok(ResponseFactory.Success(sites, "Sites retrieved successfully."));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, [FromQuery] string lang = "en")
        {
            var site = await _siteService.GetByIdAsync(id, lang);
            return Ok(ResponseFactory.Success(site, "Site retrieved successfully."));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateSiteDto dto)
        {
            var site = await _siteService.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById),
                new { id = site.Id },
                ResponseFactory.Success(site, "Site created successfully."));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateSiteDto dto)
        {
            await _siteService.UpdateAsync(id, dto);
            return Ok(ResponseFactory.Success(message: "Site updated successfully."));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _siteService.DeleteAsync(id);

            return Ok(ResponseFactory.Success("Site deleted successfully."));
        }
    }
}
