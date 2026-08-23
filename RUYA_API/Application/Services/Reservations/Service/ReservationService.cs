using Microsoft.EntityFrameworkCore;
using RUYA_API.Application.Services.Reservations.DTOs;
using RUYA_API.Application.Services.Reservations.Interfaces;
using RUYA_API.Application.Services.Reservations.Mappings;
using RUYA_API.ExceptionHandling.CustomException;
using RUYA_API.Infrastructure.Context;

namespace RUYA_API.Application.Services.Reservations.Service
{
    public class ReservationService : IReservationService
    {
        private readonly RuyaContext _context;

        public ReservationService(RuyaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ReservationDto>> GetAllAsync(string userId)
        {
            var reservations = await _context.Reservations
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.ReservationDate)
                .AsNoTracking()
                .ToListAsync();

            return reservations.Select(r => r.ToDto());
        }

        public async Task<ReservationDto> GetByIdAsync(int id, string userId)
        {
            var reservation = await _context.Reservations
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation is null)
            {
                throw new AppException(
                    "Reservation not found.",
                    StatusCodes.Status404NotFound);
            }

            if (reservation.UserId != userId)
            {
                throw new AppException(
                    "You are not allowed to access this reservation.",
                    StatusCodes.Status403Forbidden);
            }

            return reservation.ToDto();
        }

        public async Task<ReservationDto> CreateAsync(CreateReservationDto dto, string userId)
        {
            ValidateReservation(dto.MuseumName, dto.ReservationDate);

            var reservation = dto.ToEntity(userId);

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return reservation.ToDto();
        }

        public async Task UpdateAsync(int id, UpdateReservationDto dto, string userId)
        {
            ValidateReservation(dto.MuseumName, dto.ReservationDate);

            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation is null)
            {
                throw new AppException(
                    "Reservation not found.",
                    StatusCodes.Status404NotFound);
            }

            if (reservation.UserId != userId)
            {
                throw new AppException(
                    "You are not allowed to update this reservation.",
                    StatusCodes.Status403Forbidden);
            }

            dto.UpdateEntity(reservation);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id, string userId)
        {
            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation is null)
            {
                throw new AppException(
                    "Reservation not found.",
                    StatusCodes.Status404NotFound);
            }

            if (reservation.UserId != userId)
            {
                throw new AppException(
                    "You are not allowed to delete this reservation.",
                    StatusCodes.Status403Forbidden);
            }

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
        }

        private static void ValidateReservation(string museumName, DateTime reservationDate)
        {
            if (string.IsNullOrWhiteSpace(museumName))
                throw new AppException("Museum name is required.", StatusCodes.Status400BadRequest);

            if (reservationDate < DateTime.UtcNow.Date)
                throw new AppException("Reservation date cannot be in the past.", StatusCodes.Status400BadRequest);
        }
    }
}
