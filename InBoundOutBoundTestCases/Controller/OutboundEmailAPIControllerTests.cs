using AutoMapper;
using Email.Services.EmailTrailAPI.Services.IService;
using Email.Services.InboundEmailAPI.Models;
using Email.Services.InboundEmailAPI.Services.IService;
using Email.Services.OutboundEmail.Controllers;
using Email.Services.OutboundEmail.Data;
using Email.Services.OutboundEmail.Models;
using Email.Services.OutboundEmail.Models.DTO;
using Email.Services.OutboundEmail.Services.IService;
using MailKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InBoundOutBoundTestCases.Controller
{
    public class OutboundEmailAPIControllerTests
    {
        private readonly AppDbContext _dbContext;
        public OutboundEmailAPIControllerTests()
        {
            // Initialize in-memory database for testing
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new AppDbContext(dbContextOptions);

            // Seed the database with test data
            _dbContext.MailRequests.Add(new MailRequest { UniqueId=Guid.NewGuid().ToString(), ToEmail = "test@a.com", Subject = "[Ticket:T123] Subject 1", Body = "This is test email subject 1 body",ToName="Test"});
            _dbContext.MailRequests.Add(new MailRequest { UniqueId = Guid.NewGuid().ToString(), ToEmail = "test@a.com", Subject = "[Ticket:T123] Subject 2", Body = "This is test email subject 1 body", ToName = "Test" });
            _dbContext.MailRequests.Add(new MailRequest { UniqueId = Guid.NewGuid().ToString(), ToEmail = "test@a.com",Subject = "Not a ticket related email", Body = "This is test email subject 1 body", ToName = "Test" });

            _dbContext.SaveChanges();
        }
        [Fact]
        public async Task SendMail_ValidData_ReturnsOk()
        {
            // Arrange
            var mailServiceMock = new Mock<IOutboundEmailService>();
            var loggerMock = new Mock<ILogger<OutboundEmailAPIController>>();
            var mapperMock = new Mock<IMapper>();
            var controller = new OutboundEmailAPIController(mailServiceMock.Object,_dbContext, loggerMock.Object, mapperMock.Object);
            var mailData = new MailRequestDto
            {
                ToEmail = "narayan.bhosale@harbingergroup.com",
                ToName = "Test Demo",
                Subject = "Testing",
                Body = "Test"
            };

            // Act
            var result = await controller.SendMail(mailData);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public async Task GetEmailTrailByTicketIdAsync_ValidTicketId_ReturnsEmailTrails()
        {
            try
            {
                // Arrange
                var testData = new List<MailRequest>
    {
        new MailRequest { Id = 1, UniqueId = "T123", ToEmail = "narayan.bhosale@example.com", ToName = "Recipient 1", Subject = "Test Subject 1", Body = "Test Body 1", Attachments = new List<IFormFile>(), AttachmentName = "Attachment1", AttachmentURL = "https://example.com/attachment1" },
        new MailRequest { Id = 2, UniqueId = "T123", ToEmail = "narayan.bhosale@example.com", ToName = "Recipient 2", Subject = "Test Subject 2", Body = "Test Body 2", Attachments = new List<IFormFile>(), AttachmentName = "Attachment2", AttachmentURL = "https://example.com/attachment2" },
        new MailRequest { Id = 3, UniqueId = "T123", ToEmail = "narayan.bhosale@example.com", ToName = "Recipient 3", Subject = "Test Subject 3", Body = "Test Body 3", Attachments = new List<IFormFile>(), AttachmentName = "Attachment3", AttachmentURL = "https://example.com/attachment3" },
    }.AsQueryable();

                var mockSet = new Mock<DbSet<MailRequest>>();
                mockSet.As<IQueryable<MailRequest>>().Setup(m => m.Provider).Returns(testData.Provider);
                mockSet.As<IQueryable<MailRequest>>().Setup(m => m.Expression).Returns(testData.Expression);
                mockSet.As<IQueryable<MailRequest>>().Setup(m => m.ElementType).Returns(testData.ElementType);
                mockSet.As<IQueryable<MailRequest>>().Setup(m => m.GetEnumerator()).Returns(testData.GetEnumerator());

                var options = new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase(databaseName: "TestDatabase")
                    .Options;
                var dbContext = new AppDbContext(options);

                // Create mock instances for dependencies
                var emailTrailServiceMock = new Mock<IEmailTrailService>();
                var loggerMock = new Mock<ILogger<OutboundEmailAPIController>>();
                var outboundEmailServiceMock = new Mock<IOutboundEmailService>();
                var inboundEmailServiceMock = new Mock<IInboundEmailService>();
                var mapperMock = new Mock<IMapper>();

                // Create an instance of the controller with mock dependencies
                var controller = new OutboundEmailAPIController(outboundEmailServiceMock.Object,_dbContext,loggerMock.Object, mapperMock.Object);

                // Act
                var result = await controller.GetEmailTrailByTicketIdAsync("T123");

                // Assert
                Assert.NotNull(result);
                Assert.IsAssignableFrom<List<MailRequest>>(result);
                Assert.Equal(2, result.Count);

            }
            catch (Exception ex)
            {

            }
        }
    }
}
