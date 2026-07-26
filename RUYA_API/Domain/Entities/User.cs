using Microsoft.AspNetCore.Identity;
using RUYA_API.Domain.Common;

namespace RUYA_API.Domain.Entities
{
    public class User: IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public string PreferredLanguage { get; set; } = string.Empty;
        public string KnowledgeLevel { get; set; } = string.Empty;

        public ICollection<Tour> Tours { get; set; } = new List<Tour>();
        public ICollection<MemoryAlbum> MemoryAlbums { get; set; } = new List<MemoryAlbum>();
    }
}
