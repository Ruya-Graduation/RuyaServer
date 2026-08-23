using System.ComponentModel.DataAnnotations;

namespace RUYA_API.Application.Services.Reservations.DTOs
{
    public class CreateReservationDto
    {
        [Required]
        [MaxLength(200)]
        public string MuseumName { get; set; } = string.Empty;

        [Required]
        public DateTime ReservationDate { get; set; }
    }
}
