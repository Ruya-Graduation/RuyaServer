using RUYA_API.Application.Services.Reservations.DTOs;

namespace RUYA_API.Application.Services.Reservations.Interfaces
{
    public interface IReservationService
    {
        Task<IEnumerable<ReservationDto>> GetAllAsync(string userId);
        Task<ReservationDto> GetByIdAsync(int id, string userId);
        Task<ReservationDto> CreateAsync(CreateReservationDto dto, string userId);
        Task UpdateAsync(int id, UpdateReservationDto dto, string userId);
        Task DeleteAsync(int id, string userId);
    }
}
