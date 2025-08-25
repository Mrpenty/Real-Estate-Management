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

namespace RealEstateManagement.UnitTests.PropertiesTest.InterestedPropertyServiceTest
{
[TestClass]
public class ConfirmInterestAsyncTests
{
    private Mock<IInterestedPropertyRepository> _repoMock;
    private Mock<IPropertyPostRepository> _postRepoMock;
    private Mock<IMessageRepository> _msgRepoMock;
    private Mock<IRentalContractRepository> _contractRepoMock; // 👈 thêm mock

    private InterestedPropertyService _service;

    [TestInitialize]
    public void Setup()
    {
        _repoMock = new Mock<IInterestedPropertyRepository>();
        _postRepoMock = new Mock<IPropertyPostRepository>();
        _msgRepoMock = new Mock<IMessageRepository>();
        _contractRepoMock = new Mock<IRentalContractRepository>(); // 👈 khởi tạo

        _service = new InterestedPropertyService(
            _repoMock.Object,
            _postRepoMock.Object,
            _msgRepoMock.Object,
            _contractRepoMock.Object // 👈 truyền vào constructor
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
