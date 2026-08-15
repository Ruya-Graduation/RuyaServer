using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RUYA_API.Application.Common.Interfaces;
using RUYA_API.Application.Services.Admin.Interfaces;
using RUYA_API.Application.Services.Chat.DTOs;
using RUYA_API.Application.Services.Chat.Interfaces;
using RUYA_API.Domain.Entities;
using RUYA_API.ExceptionHandling.CustomException;

namespace RUYA_API.Application.Services.Chat.Service
{
    public class ChatService : IChatService
    {
        private readonly IChatSessionRepository _sessionRepository;
        private readonly IChatMessageRepository _messageRepository;
        private readonly IVisionAiClient _visionAiClient;
        private readonly IChatAiClient _chatAiClient;
        private readonly IArtifactService _artifactService;
        private readonly IImageService _imageService;
        private readonly IOptions<AIServiceSettings> _aiSettings;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<ChatService> _logger;

        public ChatService(
            IChatSessionRepository sessionRepository,
            IChatMessageRepository messageRepository,
            IVisionAiClient visionAiClient,
            IChatAiClient chatAiClient,
            IArtifactService artifactService,
            IImageService imageService,
            IOptions<AIServiceSettings> aiSettings,
            IServiceScopeFactory scopeFactory,
            ILogger<ChatService> logger)
        {
            _sessionRepository = sessionRepository;
            _messageRepository = messageRepository;
            _visionAiClient = visionAiClient;
            _chatAiClient = chatAiClient;
            _artifactService = artifactService;
            _imageService = imageService;
            _aiSettings = aiSettings;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public async Task<ChatResponseDto> SendMessageAsync(ChatRequestDto dto, string? userId)
        {
            Conversation conversation;

            // ═══════════════════════════════════════════════════════════════
            // STEP 1: Get or Create Conversation Session
            // ═══════════════════════════════════════════════════════════════
            if (dto.ConversationId.HasValue)
            {
                conversation = await _sessionRepository.GetByIdAsync(dto.ConversationId.Value);

                if (conversation is null)
                {
                    throw new AppException(
                        "Conversation not found.",
                        StatusCodes.Status404NotFound);
                }

                // Ownership validation
                if (conversation.UserId != userId)
                {
                    throw new AppException(
                        "You are not allowed to send messages to this conversation.",
                        StatusCodes.Status403Forbidden);
                }
            }
            else
            {
                conversation = new Conversation
                {
                    UserId = userId,
                    Title = dto.Message.Length > 50 ? dto.Message[..50] + "..." : dto.Message,
                    Status = "Active",
                    CurrentLanguage = dto.Language,
                    CurrentMode = dto.Mode,
                    LastMessageAt = DateTime.UtcNow
                };

                conversation = await _sessionRepository.CreateAsync(conversation);
            }

            // ═══════════════════════════════════════════════════════════════
            // STEP 2: Image Processing & Artifact Detection
            // ═══════════════════════════════════════════════════════════════
            string? detectedArtifactName = null;
            
            if (dto.Image != null)
            {
                // Save user message with image attachment
                var userMessage = new Message
                {
                    ConversationId = conversation.Id,
                    Role = "user",
                    Content = dto.Message
                };
                await _messageRepository.AddMessageAsync(userMessage);

                // Upload image to Cloudinary for persistence
                var uploadedImage = await _imageService.UploadImageAsync(dto.Image);
                var attachment = new ConversationAttachment
                {
                    ConversationId = conversation.Id,
                    MessageId = userMessage.Id,
                    FileUrl = uploadedImage.ImageUrl,
                    PublicId = uploadedImage.PublicId,
                    FileType = "image",
                    MimeType = dto.Image.ContentType,
                    IsPrimaryFrame = true
                };
                await _messageRepository.AddAttachmentAsync(attachment);

                // Call Vision AI endpoint with the actual image file
                _logger.LogInformation("Calling Vision AI for artifact detection...");
                var visionResult = await _visionAiClient.RecognizeArtifactAsync(dto.Image);

                double confidenceThreshold = _aiSettings.Value.ConfidenceThreshold;
                _logger.LogInformation(
                    "Vision AI result: artifact={ArtifactName}, confidence={Confidence}, threshold={Threshold}",
                    visionResult?.ArtifactId ?? "null",
                    visionResult?.Confidence ?? 0,
                    confidenceThreshold);

                // Check confidence threshold
                if (visionResult == null || !visionResult.IsSuccess || visionResult.Confidence < confidenceThreshold)
                {
                    // Low confidence - return clarification message
                    await _sessionRepository.UpdateAsync(conversation);

                    var clarificationResponse = new ChatResponseDto
                    {
                        ConversationId = conversation.Id,
                        CurrentArtifactId = conversation.CurrentArtifactId,
                        AssistantMessage = visionResult?.ClarificationMessage ?? 
                            "I am not confident about which artifact is shown in the image. Please take a clearer photo or specify the artifact name.",
                        UsedVision = true,
                        NeedsNewFrame = true
                    };

                    // Store clarification message asynchronously
                    _ = PersistAssistantMessageAsync(conversation.Id, clarificationResponse.AssistantMessage);

                    return clarificationResponse;
                }

                // High confidence - use detected artifact name
                detectedArtifactName = visionResult.ArtifactId;
                _logger.LogInformation("Artifact detected with high confidence: {ArtifactName}", detectedArtifactName);
            }
            else
            {
                // No image provided - just save user message
                var userMessage = new Message
                {
                    ConversationId = conversation.Id,
                    Role = "user",
                    Content = dto.Message
                };
                await _messageRepository.AddMessageAsync(userMessage);
            }

            // ═══════════════════════════════════════════════════════════════
            // STEP 3: Resolve Artifact ID (from detection or session state)
            // ═══════════════════════════════════════════════════════════════
            int? artifactId = null;
            string? artifactNameToUse = null;

            if (!string.IsNullOrEmpty(detectedArtifactName))
            {
                // New detection - find artifact by name
                artifactNameToUse = detectedArtifactName;
                artifactId = await ResolveArtifactIdByNameAsync(detectedArtifactName);
                
                if (artifactId.HasValue)
                {
                    conversation.CurrentArtifactId = artifactId.Value;
                    _logger.LogInformation(
                        "Artifact resolved: name={ArtifactName}, id={ArtifactId}",
                        detectedArtifactName,
                        artifactId);
                }
                else
                {
                    _logger.LogWarning(
                        "Artifact '{ArtifactName}' detected by vision AI but not found in database",
                        detectedArtifactName);
                }
            }
            else if (conversation.CurrentArtifactId.HasValue)
            {
                // Reuse from session
                artifactId = conversation.CurrentArtifactId;
                _logger.LogInformation("Reusing artifact from session: id={ArtifactId}", artifactId);
            }

            await _sessionRepository.UpdateAsync(conversation);

            // ═══════════════════════════════════════════════════════════════
            // STEP 4: Fetch Artifact Data for RAG Context
            // ═══════════════════════════════════════════════════════════════
            ArtifactContextDto? artifactContext = null;

            if (artifactId.HasValue)
            {
                var artifact = await _artifactService.GetByIdAsync(artifactId.Value);
                if (artifact != null)
                {
                    artifactContext = new ArtifactContextDto
                    {
                        Name = artifact.Name,
                        Period = artifact.Period ?? string.Empty,
                        Material = artifact.Material ?? string.Empty,
                        PlaceOfDiscovery = artifact.PlaceOfDiscovery ?? string.Empty
                    };
                    _logger.LogInformation("Artifact context prepared: {ArtifactName}", artifact.Name);
                }
            }

            // If no artifact context available, return error
            if (artifactContext == null)
            {
                var noArtifactResponse = new ChatResponseDto
                {
                    ConversationId = conversation.Id,
                    CurrentArtifactId = conversation.CurrentArtifactId,
                    AssistantMessage = "I need to know which artifact you're asking about. Please provide a clear image of the artifact or specify its name.",
                    UsedVision = dto.Image != null,
                    NeedsNewFrame = true
                };

                _ = PersistAssistantMessageAsync(conversation.Id, noArtifactResponse.AssistantMessage);
                return noArtifactResponse;
            }

            // ═══════════════════════════════════════════════════════════════
            // STEP 5: Fetch Conversation History
            // ═══════════════════════════════════════════════════════════════
            int historySize = _aiSettings.Value.HistoryWindowSize;
            var history = await _messageRepository.GetRecentMessagesAsync(conversation.Id, historySize);
            
            _logger.LogInformation(
                "Fetched {HistoryCount} messages from conversation history",
                history.Count);

            // ═══════════════════════════════════════════════════════════════
            // STEP 6: Call AI Conversation Endpoint (RAG)
            // ═══════════════════════════════════════════════════════════════
            var conversationRequest = new ConversationApiRequestDto
            {
                Artifact = artifactContext,
                Question = dto.Message,
                Messages = history.Select(h => new MessageContextDto
                {
                    Role = h.Role,
                    Content = h.Content
                }).ToList()
            };

            _logger.LogInformation("Calling AI conversation endpoint...");
            var aiResponse = await _chatAiClient.GetAiResponseAsync(conversationRequest);
            
            _logger.LogInformation(
                "AI response received: chunks={ChunkCount}, answer_length={AnswerLength}",
                aiResponse.RetrievedChunks,
                aiResponse.Answer.Length);

            // ═══════════════════════════════════════════════════════════════
            // STEP 7: Return Answer to Mobile Immediately
            // ═══════════════════════════════════════════════════════════════
            var responseDto = new ChatResponseDto
            {
                ConversationId = conversation.Id,
                CurrentArtifactId = conversation.CurrentArtifactId,
                AssistantMessage = aiResponse.Answer,
                UsedVision = dto.Image != null,
                NeedsNewFrame = false
            };

            // ═══════════════════════════════════════════════════════════════
            // STEP 8: Persist Assistant Response Asynchronously
            // ═══════════════════════════════════════════════════════════════
            _ = PersistAssistantMessageAsync(conversation.Id, aiResponse.Answer);

            return responseDto;
        }

        /// <summary>
        /// Resolve artifact database ID by matching the name from Vision AI
        /// </summary>
        private async Task<int?> ResolveArtifactIdByNameAsync(string artifactName)
        {
            try
            {
                _logger.LogInformation("🔍 Resolving artifact by name: '{ArtifactName}'", artifactName);
                
                // Fetch all artifacts and find by name (case-insensitive)
                var allArtifacts = await _artifactService.GetAllAsync();
                
                _logger.LogInformation("📊 Total artifacts in database: {Count}", allArtifacts.Count());
                
                // Log first 10 artifact names for debugging
                var sampleNames = allArtifacts.Take(10).Select(a => a.Name).ToList();
                _logger.LogInformation("📋 Sample artifact names from DB: {Names}", 
                    string.Join(", ", sampleNames));
                
                var matchedArtifact = allArtifacts.FirstOrDefault(a => 
                    a.Name.Equals(artifactName, StringComparison.OrdinalIgnoreCase));

                if (matchedArtifact != null)
                {
                    _logger.LogInformation("✅ Artifact matched: '{Name}' -> ID {Id}", 
                        matchedArtifact.Name, matchedArtifact.Id);
                }
                else
                {
                    _logger.LogWarning("❌ No matching artifact found for: '{ArtifactName}'", artifactName);
                    
                    // Try to find similar names for debugging
                    var similarArtifacts = allArtifacts
                        .Where(a => a.Name.Contains(artifactName, StringComparison.OrdinalIgnoreCase) ||
                                   artifactName.Contains(a.Name, StringComparison.OrdinalIgnoreCase))
                        .Take(5)
                        .Select(a => a.Name)
                        .ToList();
                    
                    if (similarArtifacts.Any())
                    {
                        _logger.LogInformation("🔍 Similar artifacts found in DB: {Names}", 
                            string.Join(", ", similarArtifacts));
                    }
                }

                return matchedArtifact?.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to resolve artifact by name: {ArtifactName}", artifactName);
                return null;
            }
        }

        /// <summary>
        /// Persist assistant message to database asynchronously without blocking response
        /// </summary>
        private Task PersistAssistantMessageAsync(int conversationId, string content)
        {
            return Task.Run(async () =>
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var msgRepo = scope.ServiceProvider.GetRequiredService<IChatMessageRepository>();
                    await msgRepo.AddMessageAsync(new Message
                    {
                        ConversationId = conversationId,
                        Role = "assistant",
                        Content = content
                    });
                    _logger.LogInformation(
                        "Assistant message persisted for conversation {ConversationId}",
                        conversationId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Failed to persist assistant message for conversation {ConversationId}",
                        conversationId);
                }
            });
        }

        public async Task<ConversationDetailsDto> GetConversationAsync(int conversationId, string? userId)
        {
            var conversation = await _sessionRepository.GetByIdAsync(conversationId);

            if (conversation is null)
            {
                throw new AppException(
                    "Conversation not found.",
                    StatusCodes.Status404NotFound);
            }

            // Ownership validation
            if (conversation.UserId != userId)
            {
                throw new AppException(
                    "You are not allowed to access this conversation.",
                    StatusCodes.Status403Forbidden);
            }

            return new ConversationDetailsDto
            {
                ConversationId = conversation.Id,
                Title = conversation.Title,
                Status = conversation.Status,
                CurrentLanguage = conversation.CurrentLanguage,
                CurrentMode = conversation.CurrentMode,
                LastMessageAt = conversation.LastMessageAt,
                Messages = conversation.Messages
                    .OrderBy(m => m.CreatedAt)
                    .Select(m => new MessageDto
                    {
                        Id = m.Id,
                        Role = m.Role,
                        Content = m.Content,
                        CreatedAt = m.CreatedAt
                    })
                    .ToList()
            };
        }

        public async Task<IEnumerable<ConversationListItemDto>> GetConversationsAsync(string? userId)
        {
            if (string.IsNullOrEmpty(userId))
                return Enumerable.Empty<ConversationListItemDto>();

            var conversations = await _sessionRepository.GetUserConversationsAsync(userId);

            return conversations.Select(c => new ConversationListItemDto
            {
                ConversationId = c.Id,
                Title = c.Title,
                Status = c.Status,
                LastMessageAt = c.LastMessageAt,
                MessageCount = c.Messages.Count
            });
        }

        public async Task DeleteConversationAsync(int conversationId, string? userId)
        {
            var conversation = await _sessionRepository.GetByIdAsync(conversationId);

            if (conversation is null)
            {
                throw new AppException(
                    "Conversation not found.",
                    StatusCodes.Status404NotFound);
            }

            // Ownership validation
            if (conversation.UserId != userId)
            {
                throw new AppException(
                    "You are not allowed to delete this conversation.",
                    StatusCodes.Status403Forbidden);
            }

            await _sessionRepository.DeleteAsync(conversation);
        }
    }
}
