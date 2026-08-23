using RUYA_API.Domain.Common;

namespace RUYA_API.Domain.Entities
{
    public class ArtifactTranslation : EntityBase
    {
        public int ArtifactId { get; set; }
        public Artifact Artifact { get; set; } = null!;

        public string LanguageCode { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Civilization { get; set; } = string.Empty;
        public string Period { get; set; } = string.Empty;
        public string Material { get; set; } = string.Empty;
        public string PlaceOfDiscovery { get; set; } = string.Empty;
    }
}
