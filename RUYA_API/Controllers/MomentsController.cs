using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RUYA_API.Application.Services.Moments.DTOs;
using RUYA_API.Application.Services.Moments.Interfaces;
using RUYA_API.Responses;
using System.Security.Claims;

namespace RUYA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MomentsController : ControllerBase
    {
        private readonly IMomentsService _momentsService;

        public MomentsController(IMomentsService momentsService)
        {
            _momentsService = momentsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAlbums()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var albums = await _momentsService.GetAlbumsAsync(userId!);
            return Ok(ResponseFactory.Success(albums, "Albums retrieved successfully."));
        }

        [HttpGet("{albumId:int}")]
        public async Task<IActionResult> GetAlbum(int albumId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var album = await _momentsService.GetAlbumByIdAsync(albumId, userId!);
            return Ok(ResponseFactory.Success(album, "Album retrieved successfully."));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAlbum([FromForm] CreateMomentAlbumDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var album = await _momentsService.CreateAlbumAsync(dto, userId!);
            return CreatedAtAction(nameof(GetAlbum), new { albumId = album.Id },
                ResponseFactory.Success(album, "Album created successfully."));
        }

        [HttpPost("{albumId:int}/photos")]
        public async Task<IActionResult> AddPhoto(int albumId, [FromForm] AddPhotoToAlbumDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var album = await _momentsService.AddPhotoAsync(albumId, dto, userId!);
            return Ok(ResponseFactory.Success(album, "Photo added successfully."));
        }

        [HttpDelete("{albumId:int}/photos/{photoId:int}")]
        public async Task<IActionResult> DeletePhoto(int albumId, int photoId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _momentsService.DeletePhotoAsync(albumId, photoId, userId!);
            return Ok(ResponseFactory.Success("Photo deleted successfully."));
        }

        [HttpPut("{albumId:int}")]
        public async Task<IActionResult> UpdateAlbum(int albumId, [FromForm] UpdateMomentAlbumDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var album = await _momentsService.UpdateAlbumAsync(albumId, dto, userId!);
            return Ok(ResponseFactory.Success(album, "Album updated successfully."));
        }

        [HttpDelete("{albumId:int}")]
        public async Task<IActionResult> DeleteAlbum(int albumId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _momentsService.DeleteAlbumAsync(albumId, userId!);
            return Ok(ResponseFactory.Success("Album deleted successfully."));
        }
    }
}
