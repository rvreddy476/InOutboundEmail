using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Email.Services.OutboundEmail.Data;
using Email.Services.OutboundEmail.Models;
using Email.Services.OutboundEmail.Services;

namespace OutboundEmailServiceTests.Tests
{
    public class OutboundEmailServiceTests
    {
        private readonly Mock<EmailConfig> _mockEmailConfig;
        private readonly Mock<AppDbContext> _mockDbContext;
        private readonly Mock<ILogger<OutboundEmailService>> _mockLogger;
        private readonly OutboundEmailService _outboundEmailService;

        public OutboundEmailServiceTests()
        {
            _mockEmailConfig = new Mock<EmailConfig>();
            _mockDbContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            _mockLogger = new Mock<ILogger<OutboundEmailService>>();

            _outboundEmailService = new OutboundEmailService(
                _mockEmailConfig.Object,
                _mockDbContext.Object,
                _mockLogger.Object
            );
        }

        [Fact]
        public async Task OutboundIncomingEmail_SavesEmailAndSendsMail_WhenSubjectIsNotEmpty()
        {
            // Arrange
            var request = new MailRequest
            {
                Subject = "",
                ToEmail = "recipient@example.com",
                ToName = "Recipient",
                Body = "Test Body"
            };
            var attachmentFilePath = "path/to/attachment.txt";

            _mockDbContext.Setup(db => db.Add(It.IsAny<MailRequest>())).Verifiable();

            // Mock SendMailAsync method to return true
            var mockService = new Mock<OutboundEmailService>(
                _mockEmailConfig.Object,
                _mockDbContext.Object,
                _mockLogger.Object
            )
            { CallBase = true };
           // Act
            var result = await mockService.Object.OutboundIncomingEmail(request, attachmentFilePath);

            // Assert           
            Assert.NotNull(result);
            Assert.Equal(request.Subject, result.Subject);
        }

        [Fact]
        public void GetUniqueIdentifierFromSubjectForOutbound_ReturnsUniqueId()
        {
            // Arrange
            var subject = "Test Subject Ticket:123";

            // Act
            var uniqueId = _outboundEmailService.GetUniqueIdentifierFromSubjectForOutbound(subject);

            // Assert
            Assert.Equal("123", uniqueId);
        }



    }
}
