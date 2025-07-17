using Microsoft.AspNetCore.Mvc;
using Moq;
using SimpleIAM_API.Controllers;
using SimpleIAM_API.DTO;
using SimpleIAM_API.Service;

namespace SimpleIAM_API_Tests
{
    public class UsersController_AssignGroupTests
    {
        private readonly Mock<IUserService> _mockService;
        private readonly UsersController _controller;

        public UsersController_AssignGroupTests()
        {
            _mockService = new Mock<IUserService>();
            _controller = new UsersController(_mockService.Object);
        }

        [Fact]
        public async Task AssignGroup_Success_ReturnsOk()
        {
            // Arrange
            var dto = new AssignGroupDto { Email = "test@example.com", GroupId = 2 };

            _mockService.Setup(s => s.AssignGroupAsync(dto.Email, dto.GroupId))
                        .Returns(Task.CompletedTask); // Simulate success

            // Act
            var result = await _controller.AssignGroup(dto);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var message = Assert.IsType<string>(ok.Value);
            Assert.Contains("assigned", message);
        }

        [Fact]
        public async Task AssignGroup_UserOrGroupNotFound_ReturnsNotFound()
        {
            // Arrange
            var dto = new AssignGroupDto { Email = "missing@example.com", GroupId = 99 };

            _mockService.Setup(s => s.AssignGroupAsync(dto.Email, dto.GroupId))
                        .ThrowsAsync(new KeyNotFoundException("User not found"));

            // Act
            var result = await _controller.AssignGroup(dto);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("User not found", notFound.Value);
        }

        [Fact]
        public async Task AssignGroup_AlreadyAssigned_ReturnsConflict()
        {
            // Arrange
            var dto = new AssignGroupDto { Email = "test@example.com", GroupId = 1 };

            _mockService.Setup(s => s.AssignGroupAsync(dto.Email, dto.GroupId))
                        .ThrowsAsync(new InvalidOperationException("User already in group"));

            // Act
            var result = await _controller.AssignGroup(dto);

            // Assert
            var conflict = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal("User already in group", conflict.Value);
        }
    }
}
