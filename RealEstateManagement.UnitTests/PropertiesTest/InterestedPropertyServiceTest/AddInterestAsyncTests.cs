using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Business.Repositories.Chat.Messages;
using RealEstateManagement.Business.Repositories.OwnerRepo;
using RealEstateManagement.Business.Repositories.Properties;
using RealEstateManagement.Business.Services.Properties;
using RealEstateManagement.Data.Entity.PropertyEntity;
using RealEstateManagement.Data.Entity.User;
using System;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.PropertiesTest.InterestedPropertyServiceTest
{
    [TestClass]
    public class AddInterestAsyncTests
    {
        protected Mock<IInterestedPropertyRepository> _interestedRepo;
        protected Mock<IPropertyPostRepository> _propertyPostRepo;
        protected Mock<IMessageRepository> _messageRepo;
        protected Mock<IRentalContractRepository> _contractRepo;
        protected InterestedPropertyService _service;

        [TestInitialize]
        public void Setup()
        {
            _interestedRepo = new Mock<IInterestedPropertyRepository>();
            _propertyPostRepo = new Mock<IPropertyPostRepository>();
            _messageRepo = new Mock<IMessageRepository>();
            _contractRepo = new Mock<IRentalContractRepository>();

            _service = new InterestedPropertyService(
                _interestedRepo.Object,
                _propertyPostRepo.Object,
                _messageRepo.Object,
                _contractRepo.Object
            );
        }

        [TestMethod]
        public async Task Adds_New_Interest_When_Not_Exists()
        {
            _interestedRepo.Setup(r => r.GetByRenterAndPropertyAsync(1, 1)).ReturnsAsync((InterestedProperty)null);
            _interestedRepo.Setup(r => r.AddAsync(It.IsAny<InterestedProperty>()))
                     .ReturnsAsync(new InterestedProperty { Id = 99, RenterId = 1, PropertyId = 1 });

            var result = await _service.AddInterestAsync(1, 1);

            Assert.AreEqual(99, result.Id);
            Assert.AreEqual(1, result.RenterId);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task Throws_When_LandlordRejected()
        {
            _interestedRepo.Setup(r => r.GetByRenterAndPropertyAsync(1, 1))
                     .ReturnsAsync(new InterestedProperty { Status = InterestedStatus.LandlordRejected });

            await _service.AddInterestAsync(1, 1);
        }

        [TestMethod]
        public async Task Reopen_When_Status_None()
        {
            var ip = new InterestedProperty { Id = 5, RenterId = 1, PropertyId = 1, Status = InterestedStatus.None };
            _interestedRepo.Setup(r => r.GetByRenterAndPropertyAsync(1, 1)).ReturnsAsync(ip);

            var result = await _service.AddInterestAsync(1, 1);

            Assert.AreEqual(InterestedStatus.WaitingForRenterReply, ip.Status);
        }
    }
}
