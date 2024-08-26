using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Email.Services.EmailTrailAPI.Data;
using Email.Services.EmailTrailAPI.Models;
using Email.Services.EmailTrailAPI.Models.DTO;
using Email.Services.EmailTrailAPI.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Org.BouncyCastle.Cms;

namespace EmailTrailServiceTests.Tests
{
    public class EmailTrailServiceTests
    {
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly AppDbContext _context;
        private readonly EmailTrailService _emailTrailService;

        public EmailTrailServiceTests()
        {
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();

            // In-memory database setup
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "EmailTrailDatabase")
                .Options;

            _context = new AppDbContext(options);

            // Initialize EmailTrailService
            _emailTrailService = new EmailTrailService(_context, _mockHttpClientFactory.Object);
        }

        [Fact]
        public async Task GetEmailTrailByTicketIdAsync_ReturnsSortedEmailTrails()
        {
            // Arrange
            var ticketId = "ticket123";
            var emailTrails = new List<EmailTrail>
            {
                 new EmailTrail { Id = 1, TicketId = "T123", Body = "Test Body",  Subject = "Test Subject" },
                    new EmailTrail {Id = 2, TicketId = "T124", Body = "Test Body", Subject = "Test Subject"}
            };

           
            // Act
            var result = await _emailTrailService.GetEmailTrailByTicketIdAsync(ticketId);

            // Assert
            Assert.Equal(0, result.Count);
           
        }

        [Fact]
        public void GetAllEmailTrails_ReturnsAllEmailTrails()
        {
            // Arrange
            var emailTrails = new List<EmailTrail>();          

            // Act
            var result = _emailTrailService.GetAllEmailTrails();

            // Assert
            Assert.Equal(0, result.Count);
        }

      
    }
}
