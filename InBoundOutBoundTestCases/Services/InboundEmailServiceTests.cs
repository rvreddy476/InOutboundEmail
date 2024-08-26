using AutoMapper;
using MailKit.Net.Imap;
using Microsoft.EntityFrameworkCore;
using Moq;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Email.Services.InboundEmailAPI.Models.DTO;
using Email.Services.InboundEmailAPI.Models;
using Email.Services.InboundEmailAPI.Services;
using MailKit;
using Email.Services.InboundEmailAPI.Data;

namespace InboundEmailServiceTests.Tests
{
    public class InboundEmailServiceTests
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<EmailConfig> _mockEmailConfig;
        private readonly List<EmailDto> _outboundEmailDto;
        private readonly Mock<AppDbContext> _mockDbContext;
        private readonly InboundEmailService _inboundEmailService;

        public InboundEmailServiceTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockEmailConfig = new Mock<EmailConfig>();
            _outboundEmailDto = new List<EmailDto>();
            _mockDbContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            _inboundEmailService = new InboundEmailService(
                _mockDbContext.Object,
                _mockMapper.Object,
                _mockEmailConfig.Object,
                _outboundEmailDto);
        }

        [Fact]
        public async Task InboundIncomingEmail_SavesEmail_WhenSubjectContainsTicketId()
        {
            // Arrange
            var request = new InboundEmail { Subject = "Ticket:123 Issue" };
            _mockDbContext.Setup(db => db.Add(It.IsAny<InboundEmail>())).Verifiable();
          
            // Act
            var result = await _inboundEmailService.InboundIncomingEmail(request);

            // Assert
            _mockDbContext.Verify();
            Assert.Equal(request, result);
        }

      
       
    }
}
