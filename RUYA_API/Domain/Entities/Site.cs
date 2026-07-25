
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

        public ICollection<Artifact> Artifacts { get; set; } = new List<Artifact>();
        public ICollection<Tour> Tours { get; set; } = new List<Tour>();
    }
}
