
using RUYA_API.Domain.Common;

namespace RUYA_API.Domain.Entities
{
    public class Site : EntityBase
    {
        public string Name { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string Hours { get; set; } = string.Empty;
        public string Ticket { get; set; } = string.Empty;
        public string Crowds { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string ImagePublicId { get; set; } = string.Empty;

        public ICollection<Artifact> Artifacts { get; set; } = new List<Artifact>();
        public ICollection<Tour> Tours { get; set; } = new List<Tour>();
    }
}
