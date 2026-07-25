using RUYA_API.Domain.Common;
using RUYA_API.Domain.Enums;

namespace RUYA_API.Domain.Entities
{
    public class Source : EntityBase
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;

        public TrustLevel TrustLevel { get; set; }

        public ICollection<Artifact> Artifacts { get; set; } = new List<Artifact>();
    }
}
