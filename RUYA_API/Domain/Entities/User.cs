using Microsoft.AspNetCore.Identity;

namespace RUYA_API.Domain.Entities
{
    public class User : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;

        public string PreferredLanguage { get; set; } = string.Empty;

        public string KnowledgeLevel { get; set; } = string.Empty;

        public ICollection<Tour> Tours { get; set; } = new List<Tour>();

        public ICollection<MemoryAlbum> MemoryAlbums { get; set; } = new List<MemoryAlbum>();

        public ICollection<Conversation> Conversations { get; set; } = new List<Conversation>();
    }
}
