using RUYA_API.Domain.Entities;

namespace RUYA_API.Application.Services.Chat.Interfaces
{
    public interface IChatSessionRepository
    {
        Task<Conversation?> GetByIdAsync(int conversationId);
        Task<Conversation> CreateAsync(Conversation conversation);
        Task UpdateAsync(Conversation conversation);
        Task<IEnumerable<Conversation>> GetUserConversationsAsync(string userId);
        Task DeleteAsync(Conversation conversation);
    }
}
