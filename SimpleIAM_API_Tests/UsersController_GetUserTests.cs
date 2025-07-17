using Microsoft.AspNetCore.Mvc;
using Moq;
using SimpleIAM_API.Controllers;
using SimpleIAM_API.DTO;
using SimpleIAM_API.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleIAM_API_Tests
{
    public class UsersController_GetUserTests
    {
        private readonly Mock<IUserService> _mockService;
        private readonly UsersController _controller;

        public UsersController_GetUserTests()
        {
            _mockService = new Mock<IUserService>();
            _controller = new UsersController(_mockService.Object);
        }

        [Fact]
        public async Task GetUser_ExistingUser_ReturnsOk()
        {
            // Arrange
            string email = "kevin@example.com";

            var userDto = new UserDto
            {
                Id = 1,
                Email = email,
                Groups = new List<string> { "Investigator", "Site Admin" }
            };

            _mockService.Setup(s => s.GetByEmailAsync(email))
                        .ReturnsAsync(userDto);

            // Act
            var result = await _controller.GetUser(email);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            dynamic? data = ok.Value;

            Assert.Equal(userDto.Id, (int)data.Id);
            Assert.Equal(userDto.Email, (string)data.Email);
            Assert.Equal(userDto.Groups, data.Groups);
        }

        [Fact]
        public async Task GetUser_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            string email = "missing@example.com";

            _mockService.Setup(s => s.GetByEmailAsync(email))
                        .ThrowsAsync(new KeyNotFoundException("User not found"));

            // Act
            var result = await _controller.GetUser(email);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("User not found", notFound.Value);
        }

        [Fact]
        public async Task GetUser_ServiceError_ReturnsConflict()
        {
            // Arrange
            string email = "crash@example.com";

            _mockService.Setup(s => s.GetByEmailAsync(email))
                        .ThrowsAsync(new InvalidOperationException("Something went wrong"));

            // Act
            var result = await _controller.GetUser(email);

            // Assert
            var conflict = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal("Something went wrong", conflict.Value);
        }

        [Fact]
        public async Task GetUsers_ReturnsListOfUserDtos()
        {
            // Arrange
            var users = new List<UserDto>
            {
                new UserDto
                {
                    Id = 1,
                    Email = "user1@example.com",
                    Groups = new List<string> { "Investigator" }
                },
                new UserDto
                {
                    Id = 2,
                    Email = "user2@example.com",
                    Groups = new List<string> { "Sponsor Representative" }
                }
            };

            _mockService.Setup(s => s.GetAllAsync())
                        .ReturnsAsync(users);

            // Act
            var result = await _controller.GetUsers();

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var actualUsers = Assert.IsType<List<UserDto>>(ok.Value);

            Assert.Equal(2, actualUsers.Count);
            Assert.Equal("user1@example.com", actualUsers[0].Email);
            Assert.Contains("Investigator", actualUsers[0].Groups);
        }

    }
}
