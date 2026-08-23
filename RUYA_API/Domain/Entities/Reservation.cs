using RUYA_API.Domain.Common;

namespace RUYA_API.Domain.Entities
{
    public class Reservation : EntityBase
    {
        public string UserId { get; set; } = string.Empty;
        public User User { get; set; } = null!;

        public string MuseumName { get; set; } = string.Empty;
        public DateTime ReservationDate { get; set; }
    }
}
