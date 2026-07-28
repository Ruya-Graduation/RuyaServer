namespace RUYA_API.Application.Services.Admin.DTOs.Artifact
{
    public class ArtifactDto
    {
        public int Id { get; set; }

        public int SiteId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public string Civilization { get; set; } = string.Empty;

        public string Period { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;
    }
}
