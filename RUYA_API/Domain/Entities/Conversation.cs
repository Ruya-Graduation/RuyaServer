using RUYA_API.Domain.Common;

namespace RUYA_API.Domain.Entities
{
    public class Conversation : EntityBase
    {
        public int StopId { get; set; }
        public TourStop TourStop { get; set; } = null!;

        public DateTime StartedAt { get; set; }

        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
