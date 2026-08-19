using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RUYA_API.Application.Services.MemoryAlbum.DTOs;
using RUYA_API.Application.Services.MemoryAlbum.Interfaces;
using RUYA_API.Responses;
using System.Security.Claims;

namespace RUYA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MemoryAlbumsController : ControllerBase
    {
        private readonly IMemoryAlbumService _memoryAlbumService;

        public MemoryAlbumsController(IMemoryAlbumService memoryAlbumService)
        {
            _memoryAlbumService = memoryAlbumService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var albums = await _memoryAlbumService.GetAllAsync();
            return Ok(ResponseFactory.Success(albums, "Memory Albums retrieved successfully."));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var album = await _memoryAlbumService.GetByIdAsync(id);
            return Ok(ResponseFactory.Success(album, "Memory Album retrieved successfully."));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMemoryAlbumDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(ResponseFactory.Failure("User not authenticated."));

            var album = await _memoryAlbumService.CreateAsync(dto, userId);

            return CreatedAtAction(nameof(GetById),
                new { id = album.Id },
                ResponseFactory.Success(album, "Memory Album created successfully."));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateMemoryAlbumDto dto)
        {
            await _memoryAlbumService.UpdateAsync(id, dto);
            return Ok(ResponseFactory.Success(message: "Memory Album updated successfully."));
        }

        [HttpPost("{id:int}/items")]
        public async Task<IActionResult> AddItem(int id, AddAlbumItemDto dto)
        {
            var item = await _memoryAlbumService.AddAlbumItemAsync(id, dto);
            return Ok(ResponseFactory.Success(item, "Album item added successfully."));
        }

        [HttpDelete("{albumId:int}/items/{itemId:int}")]
        public async Task<IActionResult> DeleteItem(int albumId, int itemId)
        {
            await _memoryAlbumService.DeleteAlbumItemAsync(albumId, itemId);
            return Ok(ResponseFactory.Success("Album item deleted successfully."));
        }
    }
}
