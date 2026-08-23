namespace RUYA_API.Application.Services.Reservations.DTOs
{
    public class ReservationDto
    {
        public int Id { get; set; }
        public string MuseumName { get; set; } = string.Empty;
        public DateTime ReservationDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
