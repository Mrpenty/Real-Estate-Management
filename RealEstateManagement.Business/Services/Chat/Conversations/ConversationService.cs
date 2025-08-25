using RealEstateManagement.Business.DTO.Chat;
using RealEstateManagement.Business.DTO.NotificationDTO;
using RealEstateManagement.Business.Repositories.Chat.Conversations;
using RealEstateManagement.Business.Repositories.OwnerRepo;
using RealEstateManagement.Business.Repositories.Properties;
using RealEstateManagement.Business.Services.Chat.Messages;
using RealEstateManagement.Business.Services.Mail;
using RealEstateManagement.Business.Services.NotificationService;
using RealEstateManagement.Business.Services.User;
using RealEstateManagement.Data.Entity.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Services.Chat.Conversations
{
    public class ConversationService : IConversationService
    {
        private readonly IConversationRepository _repository;
        private readonly IPropertyPostRepository _postRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMessageService _messageService;
        private readonly INotificationService _notificationService;
        private readonly IMailService _mailService;
        private readonly IProfileService _user;

        public ConversationService(IConversationRepository repository, IPropertyPostRepository postRepository, IMessageService messageService, IPropertyRepository propertyRepository, INotificationService notificationService, IMailService mailService, IProfileService profileService)
        {
            _repository = repository;
            _postRepository = postRepository;
            _messageService = messageService;
            _propertyRepository = propertyRepository;
            _notificationService = notificationService;
            _mailService = mailService;
            _user = profileService;
        }
        public async Task<Conversation> CreateConversationAsync(CreateConversationDTO dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            // Không cho phép tự chat với chính mình
            if (dto.RenterId == dto.LandlordId)
                throw new ArgumentException("Renter and Landlord cannot be the same user.", nameof(dto));
            var existing = await _repository.GetByUsersAsync(dto.RenterId, dto.LandlordId, dto.PropertyId);
            if (existing != null)
            {
                return existing;
            }
            var conversation = new Conversation
            {
                RenterId = dto.RenterId,
                LandlordId = dto.LandlordId,
                PropertyId = dto.PropertyId,
                CreatedAt = DateTime.UtcNow,
            };

            return await _repository.CreateAsync(conversation);
        }
        //public async Task<Conversation> GetDetailsAsync(int id)
        //{
        //    return await _repository.GetByIdWithDetailsAsync(id);
        //}
        public async Task<IEnumerable<ConversationDTO>> GetAllByUserIdAsync(int userId)
        {
            var convs = await _repository.GetAllByUserIdAsync(userId)
            ?? Enumerable.Empty<Conversation>(); // <- chống null
            //var conversations =  await _repository.GetAllByUserIdAsync(userId);
            return convs.Select(c => new ConversationDTO
            {
                Id = c.Id,
                RenterId = c.RenterId,
                LandlordId = c.LandlordId,
                LastMessage = c.LastMessage,
                LastSentAt = c.LastSentAt,
                RenterName = userId == c.LandlordId ? c.Renter.Name ?? c.Renter.Email : null,
                LandlordName = userId == c.RenterId ? c.Landlord.Name ?? c.Landlord.Email : null
            });
        }
        public async Task<IEnumerable<ConversationDTO?>> FilterConversationAsync(int userId, string searchTerm, int skip = 0, int take = 5)
        {
            var conversations = await _repository.FilterConversationAsync(userId, searchTerm, skip, take);

            return conversations.Select(c => new ConversationDTO
            {
                Id = c.Id,
                PropertyId = c.PropertyId,
                RenterId = c.RenterId,
                LandlordId = c.LandlordId,
                RenterName = c.Renter?.Name,
                LandlordName = c.Landlord?.Name,

            });
        }

        public async Task HandleInterestAsync(int renterId, int postId)
        {
            // Lấy thông tin bài đăng
            var post = await _postRepository.GetByIdAsync(postId);
            if (post == null)
                throw new Exception("Bài đăng không tồn tại");

            var landlordId = post.LandlordId;
            var property = await _propertyRepository.GetPropertyByIdAsync(post.PropertyId);
            var content = $"Tôi quan tâm đến bài đăng của bạn (Title: {property.Title}).";

            var landlord = await _user.GetUserBasicInfoAsync(landlordId);
            if (landlord == null || string.IsNullOrEmpty(landlord.Email))
                throw new Exception("Không tìm thấy landlord hoặc landlord chưa có email");

            var existingConversation = await _repository.GetConvesationAsync(renterId, landlordId);
            if (existingConversation != null)
            {
                // Gửi tin nhắn qua hàm có sẵn
                await _messageService.SendMessageAsync(new MessageDTO
                {
                    ConversationId = existingConversation.Id,
                    Content = content
                }, renterId);

                // Thông báo cho landlord
                await _notificationService.SendNotificationToSpecificUsersAsync(new CreateNotificationDTO
                {
                    Title = "Có người quan tâm bài đăng của bạn",
                    Content = $"Một người thuê vừa quan tâm tới bài đăng: {property.Title}",
                    Type = "info",
                    Audience = "specific",
                    SpecificUserIds = new List<int> { landlordId }
                });

                await _mailService.SendEmailAsync(
                   landlord.Email,
                   "Có người quan tâm tới bài đăng của bạn",
                   $"Xin chào {landlord.Name},<br/><br/>" +
                   $"Một người thuê vừa quan tâm tới bài đăng <b>{property.Title}</b> của bạn.<br/>" +
                   $"Hãy đăng nhập vào hệ thống để xem chi tiết.<br/><br/>" +
                   $"Trân trọng,<br/>Đội ngũ BĐS Management"
               );

                return;
            }

            var conversation = await CreateConversationAsync(new CreateConversationDTO
            {
                RenterId = renterId,
                LandlordId = landlordId,
                PropertyId = post.PropertyId
            });

            if (conversation == null)
                throw new Exception("Không thể tạo hoặc lấy conversation");

            // Gửi tin nhắn qua hàm có sẵn
            await _messageService.SendMessageAsync(new MessageDTO
            {
                ConversationId = conversation.Id,
                Content = content
            }, renterId);

            // Thông báo cho landlord
            await _notificationService.SendNotificationToSpecificUsersAsync(new CreateNotificationDTO
            {
                Title = "Có người quan tâm bài đăng của bạn",
                Content = $"Một người thuê vừa quan tâm tới bài đăng: {property.Title}",
                Type = "info",
                Audience = "specific",
                SpecificUserIds = new List<int> { landlordId }
            });

            await _mailService.SendEmailAsync(
                landlord.Email,
                "Có người quan tâm tới bài đăng của bạn",
                $"Xin chào {landlord.Name},<br/><br/>" +
                $"Một người thuê vừa quan tâm tới bài đăng <b>{property.Title}</b> của bạn.<br/>" +
                $"Hãy đăng nhập vào hệ thống để xem chi tiết.<br/><br/>" +
                $"Trân trọng,<br/>Đội ngũ BĐS Management"
            );
        }
    }
}
