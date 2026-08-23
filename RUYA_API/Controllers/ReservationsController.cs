using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RUYA_API.Application.Services.Reservations.DTOs;
using RUYA_API.Application.Services.Reservations.Interfaces;
using RUYA_API.Responses;
using System.Security.Claims;

namespace RUYA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationsController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var reservations = await _reservationService.GetAllAsync(userId!);
            return Ok(ResponseFactory.Success(reservations, "Reservations retrieved successfully."));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var reservation = await _reservationService.GetByIdAsync(id, userId!);
            return Ok(ResponseFactory.Success(reservation, "Reservation retrieved successfully."));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateReservationDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var reservation = await _reservationService.CreateAsync(dto, userId!);

            return CreatedAtAction(nameof(GetById),
                new { id = reservation.Id },
                ResponseFactory.Success(reservation, "Reservation created successfully."));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateReservationDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _reservationService.UpdateAsync(id, dto, userId!);
            return Ok(ResponseFactory.Success(message: "Reservation updated successfully."));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _reservationService.DeleteAsync(id, userId!);
            return Ok(ResponseFactory.Success("Reservation deleted successfully."));
        }
    }
}
