using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Business.Services.Properties;
using RealEstateManagement.Data.Entity.PropertyEntity;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using RealEstateManagement.Business.Repositories.Chat.Messages;
using RealEstateManagement.Business.Repositories.OwnerRepo;
using RealEstateManagement.Business.Repositories.Properties;
using RealEstateManagement.Data.Entity.User;
using RealEstateManagement.Business.Services.Mail;
using RealEstateManagement.Business.Services.NotificationService;
using RealEstateManagement.Business.Services.User;
using RealEstateManagement.Business.DTO.UserDTO;

namespace RealEstateManagement.UnitTests.PropertiesTest.InterestedPropertyServiceTest
{
[TestClass]
public class ConfirmInterestAsyncTests
{
        private Mock<IInterestedPropertyRepository> _repoMock = null!;
        private Mock<IPropertyPostRepository> _postRepoMock = null!;
        private Mock<IMessageRepository> _msgRepoMock = null!;
        private Mock<IRentalContractRepository> _contractRepoMock = null!;
        private Mock<INotificationService> _notiSvcMock = null!;
        private Mock<IPropertyRepository> _propertyRepoMock = null!;
        private Mock<IMailService> _mailSvcMock = null!;
        private Mock<IProfileService> _profileSvcMock = null!;

        private InterestedPropertyService _service = null!;

        [TestInitialize]
        public void Setup()
        {
            // Dùng Loose để không cần setup những call không đụng tới
            _repoMock = new Mock<IInterestedPropertyRepository>(MockBehavior.Loose);
            _postRepoMock = new Mock<IPropertyPostRepository>(MockBehavior.Loose);
            _msgRepoMock = new Mock<IMessageRepository>(MockBehavior.Loose);
            _contractRepoMock = new Mock<IRentalContractRepository>(MockBehavior.Loose);
            _notiSvcMock = new Mock<INotificationService>(MockBehavior.Loose);
            _propertyRepoMock = new Mock<IPropertyRepository>(MockBehavior.Loose);
            _mailSvcMock = new Mock<IMailService>(MockBehavior.Loose);
            _profileSvcMock = new Mock<IProfileService>(MockBehavior.Loose);

            // Nếu service có gửi mail/notify, trả về user giả có email để tránh null
            _profileSvcMock
                .Setup(p => p.GetUserBasicInfoAsync(It.IsAny<int>()))
                .ReturnsAsync(new UserBasicInfoDto
                {
                    Id = 999,
                    Name = "Test User",
                    Email = "test@local"
                });


            // Nếu service gọi các hàm update/add, cho phép chạy trơn
            _repoMock
                .Setup(r => r.UpdateAsync(It.IsAny<InterestedProperty>()))
                .Returns(Task.CompletedTask);

            _repoMock
                .Setup(r => r.DeleteAsync(It.IsAny<InterestedProperty>()))
                .Returns(Task.CompletedTask);


            _postRepoMock.Setup(p => p.UpdateAsync(It.IsAny<PropertyPost>())).Returns(Task.CompletedTask);

            _service = new InterestedPropertyService(
                _repoMock.Object,
                _postRepoMock.Object,
                _msgRepoMock.Object,
                _contractRepoMock.Object,
                _notiSvcMock.Object,
                _propertyRepoMock.Object,
                _mailSvcMock.Object,
                _profileSvcMock.Object
            );
        }

        [TestMethod]
    [ExpectedException(typeof(System.Exception))]
    public async Task Throws_When_NotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((InterestedProperty)null);
        await _service.ConfirmInterestAsync(1, true, true);
    }

    [TestMethod]
    public async Task Renter_Confirms_True_Changes_Status_To_WaitingForLandlord()
    {
        var ip = new InterestedProperty { Id = 1, Status = InterestedStatus.WaitingForRenterReply };
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(ip);

        var result = await _service.ConfirmInterestAsync(1, true, true);

        Assert.IsTrue(result);
        Assert.AreEqual(InterestedStatus.WaitingForLandlordReply, ip.Status);
    }

    [TestMethod]
    public async Task Renter_Confirms_False_Sets_Status_None()
    {
        var ip = new InterestedProperty { Id = 1, Status = InterestedStatus.WaitingForRenterReply };
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(ip);

        var result = await _service.ConfirmInterestAsync(1, true, false);

        Assert.IsTrue(result);
        Assert.AreEqual(InterestedStatus.None, ip.Status);
    }

    [TestMethod]
    public async Task Landlord_Rejects_Sets_Status_LandlordRejected()
    {
        var ip = new InterestedProperty { Id = 1, Status = InterestedStatus.WaitingForLandlordReply };
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(ip);

        var result = await _service.ConfirmInterestAsync(1, false, false);

        Assert.IsTrue(result);
        Assert.AreEqual(InterestedStatus.LandlordRejected, ip.Status);
    }

    [TestMethod]
    public async Task Landlord_Confirms_Sets_DealSuccess_And_Closes_Others()
    {
        var ip = new InterestedProperty { Id = 1, PropertyId = 10, Status = InterestedStatus.WaitingForLandlordReply };
        var others = new List<InterestedProperty>
            {
                new InterestedProperty { Id = 2, PropertyId = 10, Status = InterestedStatus.WaitingForRenterReply }
            };

        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(ip);
        _repoMock.Setup(r => r.GetByPropertyAsync(10)).ReturnsAsync(others);

        var post = new PropertyPost { Id = 1, PropertyId = 10 };
        _postRepoMock.Setup(p => p.GetByPropertyIdAsync(10)).ReturnsAsync(post);

        var result = await _service.ConfirmInterestAsync(1, false, true);

        Assert.IsTrue(result);
        Assert.AreEqual(InterestedStatus.DealSuccess, ip.Status);
        Assert.AreEqual(PropertyPost.PropertyPostStatus.Rented, post.Status);
        Assert.AreEqual(InterestedStatus.None, others.First().Status);
    }
}
}
