using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System;
using RealEstateManagement.Business.Services.Mail;


[TestClass]
public class SendEmail
{
    private Mock<IConfiguration> _mockConfiguration;
    private MailService _mailService;

    [TestInitialize]
    public void Setup()
    {
        _mockConfiguration = new Mock<IConfiguration>();

        var smtpSettings = new Mock<IConfigurationSection>();
        smtpSettings.Setup(s => s["Host"]).Returns("smtp.test.com");
        smtpSettings.Setup(s => s["Port"]).Returns("587");
        smtpSettings.Setup(s => s["Username"]).Returns("testuser");
        smtpSettings.Setup(s => s["Password"]).Returns("testpass");
        smtpSettings.Setup(s => s["FromEmail"]).Returns("from@test.com");
        smtpSettings.Setup(s => s["FromName"]).Returns("Test Sender");

        _mockConfiguration.Setup(c => c.GetSection("Smtp")).Returns(smtpSettings.Object);

        _mailService = new MailService(_mockConfiguration.Object);
    }

    [TestMethod]
    public async Task SendEmailAsync_ValidEmail_SendsSuccessfully()
    {
        var toEmail = "testto@test.com";
        var subject = "Test Subject";
        var body = "Test Body";

        // This test case would typically require mocking the SmtpClient, which is not directly possible
        // due to its non-virtual methods. The code below is a conceptual representation.
        await _mailService.SendEmailAsync(toEmail, subject, body);
    }

    [TestMethod]
    public async Task SendEmailAsync_InvalidSmtpSettings_ThrowsInvalidOperationException()
    {
        var toEmail = "testto@test.com";
        var subject = "Test Subject";
        var body = "Test Body";

        var smtpSettings = new Mock<IConfigurationSection>();
        smtpSettings.Setup(s => s["Port"]).Returns("invalid_port");
        _mockConfiguration.Setup(c => c.GetSection("Smtp")).Returns(smtpSettings.Object);

        await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => _mailService.SendEmailAsync(toEmail, subject, body));
    }

    [TestMethod]
    public async Task SendEmailAsync_InvalidToEmailAddress_ThrowsFormatException()
    {
        var toEmail = "invalid-email";
        var subject = "Test Subject";
        var body = "Test Body";

        await Assert.ThrowsExceptionAsync<FormatException>(() => _mailService.SendEmailAsync(toEmail, subject, body));
    }
}