using Moq;
using RealEstateManagement.Business.Repositories.Chat.Conversations;
using RealEstateManagement.Business.Repositories.OwnerRepo;
using RealEstateManagement.Business.Repositories.Properties;
using RealEstateManagement.Business.Services.Chat.Conversations;
using RealEstateManagement.Business.Services.Chat.Messages;
using RealEstateManagement.Business.Services.Mail;
using RealEstateManagement.Business.Services.NotificationService;
using RealEstateManagement.Business.Services.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.ChatTest.ConversationTest
{
    public abstract class ConversationTestBase
    {
        protected Mock<IConversationRepository> Repo = null!;
        protected Mock<IPropertyPostRepository> PostRepo = null!;
        protected Mock<IMessageService> MessageSvc = null!;
        protected Mock<IPropertyRepository> PropertyRepo = null!;
        protected Mock<INotificationService> NotiSvc = null!;
        protected Mock<IMailService> MailSvc = null!;
        protected Mock<IProfileService> ProfileSvc = null!;
        protected ConversationService Svc = null!;

        [TestInitialize]
        public void Init()
        {
            // Dùng Loose để không cần setup những call không đụng tới trong test
            Repo = new Mock<IConversationRepository>(MockBehavior.Loose);
            PostRepo = new Mock<IPropertyPostRepository>(MockBehavior.Loose);
            MessageSvc = new Mock<IMessageService>(MockBehavior.Loose);
            PropertyRepo = new Mock<IPropertyRepository>(MockBehavior.Loose);
            NotiSvc = new Mock<INotificationService>(MockBehavior.Loose);
            MailSvc = new Mock<IMailService>(MockBehavior.Loose);
            ProfileSvc = new Mock<IProfileService>(MockBehavior.Loose);

            Svc = new ConversationService(
                Repo.Object,
                PostRepo.Object,
                MessageSvc.Object,
                PropertyRepo.Object,
                NotiSvc.Object,
                MailSvc.Object,
                ProfileSvc.Object
            );
        }
    }
}
