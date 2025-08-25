using Microsoft.Extensions.Logging;
using Moq;
using OfficeOpenXml;
using RealEstateManagement.Business.Repositories.Admin;
using RealEstateManagement.Business.Services.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.Admin.AdminDashBoardTest
{
    public abstract class AdminDashboardServiceTestBase
    {
        protected Mock<IAdminDashboardRepository> Repo = null!;
        protected Mock<ILogger<AdminDashboardService>> Logger = null!;
        protected AdminDashboardService Svc = null!;
        public TestContext TestContext { get; set; } = null!;
        [TestInitialize]
        public void Init()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            Repo = new Mock<IAdminDashboardRepository>(MockBehavior.Strict);
            Logger = new Mock<ILogger<AdminDashboardService>>();
            Logger.Setup(x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()))
            .Callback((LogLevel level, EventId id, object state, Exception? ex, Delegate formatter) =>
            {
                var msg = formatter.DynamicInvoke(state, ex) as string;
                TestContext.WriteLine($"[LOG {level}] {msg}");
                if (ex != null) TestContext.WriteLine($"[EXCEPTION] {ex.GetType().Name}: {ex.Message}");
            });
            Svc = new AdminDashboardService(Repo.Object, Logger.Object);
        }

        protected static void VerifyErrorLogged(Mock<ILogger<AdminDashboardService>> logger, string contains, Times times)
        {
            logger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v!.ToString()!.Contains(contains)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                times);
        }
    }
}
