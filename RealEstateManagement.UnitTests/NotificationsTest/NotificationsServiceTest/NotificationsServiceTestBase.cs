using Moq;
using RealEstateManagement.Business.Repositories.Chat.Conversations;
using RealEstateManagement.Business.Repositories.NotificationRepository;
using RealEstateManagement.Business.Services.Chat.Conversations;
using RealEstateManagement.Business.Services.NotificationService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.NotificationsTest.NotificationsServiceTest
{
    public abstract class NotificationsServiceTestBase
    {
        protected Mock<INotificationRepository> Repo = null!;
        protected NotificationService Svc = null!;

        [TestInitialize]
        public void Init()
        {
            Repo = new Mock<INotificationRepository>(MockBehavior.Strict);
            Svc = new NotificationService(Repo.Object);
        }
    }
}
