using Moq;
using RealEstateManagement.Business.Repositories.Chat.Messages;
using RealEstateManagement.Business.Services.Chat.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.ChatTest.MessageTest
{
    public abstract class MessageTestBase
    {
        protected Mock<IMessageRepository> Repo = null!;
        protected MessageService Svc = null!;

        [TestInitialize]
        public void Init()
        {
            Repo = new Mock<IMessageRepository>(MockBehavior.Strict);
            Svc = new MessageService(Repo.Object);
        }
    }
}
