namespace RUYA_API.Application.Services.Admin.DTOs.Site
{
    public class SiteDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;

        public float Latitude { get; set; }

        public float Longitude { get; set; }

        public string Hours { get; set; } = string.Empty;

        public string Ticket { get; set; } = string.Empty;

        public string Crowds { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}
