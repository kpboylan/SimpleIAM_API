using Xunit;
using Moq;
using SimpleIAM_API.Service;
using SimpleIAM_API.Controllers;
using Microsoft.AspNetCore.Mvc;
using SimpleIAM_API.DTO;
using SimpleIAM_API.Entity;

namespace SimpleIAM_API_Tests
{
    public class UsersControllerTests
    {
        private readonly Mock<IUserService> _mockService;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _mockService = new Mock<IUserService>();
            _controller = new UsersController(_mockService.Object);
        }

        [Fact]
        public async Task Register_ValidCredentials_ReturnsOkResultWithUser()
        {
            // Arrange
            var dto = new RegisterUserDto
            {
                Email = "test@example.com",
                Password = "MyCats@123"
            };

            var expectedUser = new UserDto
            {
                Id = 1,
                Email = dto.Email,
                Groups = new List<string>()
            };

            _mockService.Setup(s => s.RegisterAsync(dto.Email, dto.Password))
                        .ReturnsAsync(expectedUser);

            // Act
            var result = await _controller.Register(dto);

            // Assert
            var okResult = Assert.IsType<CreatedAtActionResult>(result);
            var actualUser = Assert.IsType<UserDto>(okResult.Value);
            Assert.Equal(expectedUser.Email, actualUser.Email);
        }

        [Fact]
        public async Task Register_InvalidCredentials_ReturnsBadRequest()
        {
            // Arrange
            var dto = new RegisterUserDto
            {
                Email = "invalid",
                Password = "123"
            };

            _mockService.Setup(s => s.RegisterAsync(dto.Email, dto.Password))
                        .ThrowsAsync(new ArgumentException("Invalid email or password."));

            // Act
            var result = await _controller.Register(dto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid email or password.", badRequest.Value);
        }

        [Fact]
        public async Task Authenticate_ValidCredentials_ReturnsOkWithUserDto()
        {
            // Arrange
            var mockService = new Mock<IUserService>();
            var controller = new UsersController(mockService.Object);

            var inputDto = new AuthenticateUserDto
            {
                Email = "test@example.com",
                Password = "Valid1@"
            };

            var userEntity = new User
            {
                Id = 1,
                Email = inputDto.Email,
                Groups = new List<UserGroup>
        {
            new UserGroup
            {
                Group = new Group { Id = 1, Name = "Investigator" }
            }
        }
            };

            mockService.Setup(s => s.AuthenticateAsync(inputDto.Email, inputDto.Password))
                       .ReturnsAsync(userEntity);

            // Act
            var result = await controller.Authenticate(inputDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<AuthenticatedUserDto>(okResult.Value);
            Assert.Equal(userEntity.Email, response.Email);
            Assert.Single(response.Groups);
            Assert.Equal("Investigator", response.Groups[0]);
        }

        [Fact]
        public async Task Authenticate_InvalidCredentials_ReturnsConflict()
        {
            // Arrange
            var mockService = new Mock<IUserService>();
            var controller = new UsersController(mockService.Object);

            var inputDto = new AuthenticateUserDto
            {
                Email = "invalid@example.com",
                Password = "WrongPass1!"
            };

            mockService.Setup(s => s.AuthenticateAsync(inputDto.Email, inputDto.Password))
                       .ThrowsAsync(new InvalidOperationException("Invalid username or password."));

            // Act
            var result = await controller.Authenticate(inputDto);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal("Invalid username or password.", conflictResult.Value);
        }

    }
}
