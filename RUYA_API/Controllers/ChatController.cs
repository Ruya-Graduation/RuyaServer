using Microsoft.AspNetCore.Mvc;
using RUYA_API.Application.Services.Chat.DTOs;
using RUYA_API.Application.Services.Chat.Interfaces;
using RUYA_API.Responses;
using System.Security.Claims;

namespace RUYA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost("message")]
        public async Task<IActionResult> SendMessage([FromForm] ChatRequestDto request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var response = await _chatService.SendMessageAsync(request, userId);

            return Ok(ResponseFactory.Success(
                response,
                "Message sent successfully."));
        }

        [HttpGet("{conversationId:int}")]
        public async Task<IActionResult> GetConversation(int conversationId)
        {
            var conversation = await _chatService.GetConversationAsync(conversationId);

            return Ok(
                ResponseFactory.Success(
                    conversation,
                    "Conversation loaded successfully."));
        }

        [HttpGet]
        public async Task<IActionResult> GetConversations()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var conversations = await _chatService.GetConversationsAsync(userId);

            return Ok(
                ResponseFactory.Success(
                    conversations,
                    "Conversations loaded successfully."));
        }

        [HttpDelete("{conversationId:int}")]
        public async Task<IActionResult> DeleteConversation(int conversationId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _chatService.DeleteConversationAsync(conversationId, userId);

            return Ok(
                ResponseFactory.Success(
                    message: "Conversation deleted successfully."));
        }
    }
}
