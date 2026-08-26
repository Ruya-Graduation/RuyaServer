using RUYA_API.Domain.Common;

namespace RUYA_API.Domain.Entities
{
    public class SiteTranslation : EntityBase
    {
        public int SiteId { get; set; }
        public Site Site { get; set; } = null!;

        public string LanguageCode { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Hours { get; set; } = string.Empty;
        public string Ticket { get; set; } = string.Empty;
        public string Crowds { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
