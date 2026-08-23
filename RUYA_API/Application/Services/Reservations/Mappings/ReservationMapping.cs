using RUYA_API.Application.Services.Reservations.DTOs;
using RUYA_API.Domain.Entities;

namespace RUYA_API.Application.Services.Reservations.Mappings
{
    public static class ReservationMapping
    {
        public static ReservationDto ToDto(this Reservation reservation)
        {
            return new ReservationDto
            {
                Id = reservation.Id,
                MuseumName = reservation.MuseumName,
                ReservationDate = reservation.ReservationDate,
                CreatedAt = reservation.CreatedAt
            };
        }

        public static Reservation ToEntity(this CreateReservationDto dto, string userId)
        {
            return new Reservation
            {
                UserId = userId,
                MuseumName = dto.MuseumName,
                ReservationDate = dto.ReservationDate
            };
        }

        public static void UpdateEntity(this UpdateReservationDto dto, Reservation reservation)
        {
            reservation.MuseumName = dto.MuseumName;
            reservation.ReservationDate = dto.ReservationDate;
        }
    }
}
