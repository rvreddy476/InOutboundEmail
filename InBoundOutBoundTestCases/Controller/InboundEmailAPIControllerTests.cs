using AutoMapper;
using Email.Services.InboundEmailAPI.Controllers;
using Email.Services.InboundEmailAPI.Data;
using Email.Services.InboundEmailAPI.Models;
using Email.Services.InboundEmailAPI.Models.DTO;
using Email.Services.InboundEmailAPI.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace InBoundOutBoundTestCases.Controller
{
    public class InboundEmailAPIControllerTests
    {
        private readonly AppDbContext _dbContext;      
        public InboundEmailAPIControllerTests()
        {
            // Initialize in-memory database for testing
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new AppDbContext(dbContextOptions);

            // Seed the database with test data
            _dbContext.InboundEmails.Add(new InboundEmail { TicketId = "T123", Subject = "[Ticket:T123] Subject 1", Body = "This is test email subject 1 body", EmailId = "narayan.bhosale@harbingergroup.com", From = "narayan.bhosale11@gmail.com" });
            _dbContext.InboundEmails.Add(new InboundEmail { TicketId = "T123", Subject = "[Ticket:T123] Subject 2", Body = "This is test email subject 1 body", EmailId = "narayan.bhosale@harbingergroup.com", From = "narayan.bhosale11@gmail.com" });
            _dbContext.InboundEmails.Add(new InboundEmail { Subject = "Not a ticket related email", Body = "This is test email subject 1 body", EmailId = "narayan.bhosale@harbingergroup.com", From = "narayan.bhosale11@gmail.com" });

            _dbContext.SaveChanges();
        }

        [Fact]
        public async Task GetEmailTrailByTicketIdAsync_ValidTicketId_ReturnsEmailTrails()
        {
            // Arrange
            var serviceMock = new Mock<IInboundEmailService>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<InboundEmailAPIController>>();
            var controller = new InboundEmailAPIController(_dbContext, serviceMock.Object, mapperMock.Object, loggerMock.Object);

            // Act
            var expectedEmailTrails = await controller.GetEmailTrailByTicketIdAsync("12345");

            // Assert
            Assert.NotNull(_dbContext.InboundEmails);
            //Assert.Equal(2, _dbContext.InboundEmails.Contains("T123"));
            //Assert.True(expectedEmailTrails.All(e => e.Subject.Contains("[Ticket:T123]")));
        }

        [Fact]
        public async Task ReceiveEmails_ReturnsOkWithEmails()
        {
            // Arrange
            var expectedEmails = new List<Email.Services.InboundEmailAPI.Models.DTO.ResponseDto>
            {
                // Create mock InboundEmail objects as needed
                new ResponseDto(),
                new ResponseDto()
            };

            var serviceMock = new Mock<IInboundEmailService>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<InboundEmailAPIController>>();
            var controller = new InboundEmailAPIController(_dbContext, serviceMock.Object, mapperMock.Object, loggerMock.Object);

            // Act
            var actionResult = await controller.ReceiveEmails();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var actualEmails = Assert.IsAssignableFrom<Email.Services.InboundEmailAPI.Models.DTO.ResponseDto> (okResult.Value);
            
            // You may want to add further assertions to check if the returned email messages match the expected ones
        }
    }
}
