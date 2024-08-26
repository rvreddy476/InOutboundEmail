using AutoMapper;
using Email.Services.EmailTrailAPI.Controllers;
using Email.Services.EmailTrailAPI.Models.DTO;
using Email.Services.EmailTrailAPI.Services.IService;

using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Email.Services.OutboundEmail.Services.IService;
using Email.Services.InboundEmailAPI.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace InBoundOutBoundTestCases.Controller
{
    public class EmailTrailControllerTests
    {
        private Mock<IMapper> _mockMapper;

        [Fact]
        public async Task GetEmailTrailsByTicketId_InvalidTicketId_ReturnsNotFound()
        {
            // Arrange
            var ticketId = "T1234";
            var emailServiceMock = new Mock<IEmailTrailService>();
            emailServiceMock.Setup(x => x.GetOutBoundEmails(ticketId)).ReturnsAsync(new List<MailSettingsDto>());

            emailServiceMock.Setup(x => x.GetInBoundEmails(ticketId)).ReturnsAsync(new List<InboundEmailDto>());

            var loggerMock = new Mock<ILogger<EmailTrailController>>();
            var emailTrailServiceMock = new Mock<IEmailTrailService>();

            _mockMapper = new Mock<IMapper>();

            var controller = new EmailTrailController(loggerMock.Object, _mockMapper.Object, emailServiceMock.Object);

            // Act
            var result = await controller.GetEmailTrailsByTicketId(ticketId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.NotEqual("No email trails found for the given ticket ID.", notFoundResult.Value);
        }

        [Fact]
        public async Task GetEmailTrailsByTicketId_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var ticketId = "T123";
            var emailServiceMock = new Mock<IEmailTrailService>();
            emailServiceMock.Setup(x => x.GetOutBoundEmails(ticketId)).ReturnsAsync(new List<MailSettingsDto>());

            var loggerMock = new Mock<ILogger<EmailTrailController>>();
            var emailTrailServiceMock = new Mock<IEmailTrailService>();


            var controller = new EmailTrailController(loggerMock.Object, _mockMapper.Object, emailServiceMock.Object);

            // Act
            var result = await controller.GetEmailTrailsByTicketId(ticketId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }
    }


}
