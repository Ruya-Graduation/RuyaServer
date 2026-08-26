
using RUYA_API.Domain.Common;

namespace RUYA_API.Domain.Entities
{
    public class Site : EntityBase
    {
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string ImagePublicId { get; set; } = string.Empty;

        public ICollection<SiteTranslation> Translations { get; set; } = new List<SiteTranslation>();
        public ICollection<Artifact> Artifacts { get; set; } = new List<Artifact>();
        public ICollection<Tour> Tours { get; set; } = new List<Tour>();
    }
}
