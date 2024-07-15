using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagementAPI.Data;
using UserManagementAPI.Models;
using UserManagementAPI.Services;
using Xunit;

namespace UserManagementAPI.Tests.UnitTests
{
    public class UserServiceTests
    {
        private readonly UserService _userService;
        private readonly ApplicationDbContext _context;

        public UserServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _userService = new UserService(_context);
        }

        [Fact]
        public async Task GetUsersAsync_ReturnsUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = 1, Username = "user1", Email = "user1@example.com" },
                new User { Id = 2, Username = "user2", Email = "user2@example.com" }
            };
            _context.Users.AddRange(users);
            _context.SaveChanges();

            // Act
            var result = await _userService.GetUsersAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetUserByIdAsync_ReturnsUser()
        {
            // Arrange
            var user = new User { Id = 1, Username = "user1", Email = "user1@example.com" };
            _context.Users.Add(user);
            _context.SaveChanges();

            // Act
            var result = await _userService.GetUserByIdAsync(1);

            // Assert
            Assert.Equal(user.Username, result.Username);
        }

        [Fact]
        public async Task CreateUserAsync_AddsUser()
        {
            // Arrange
            var user = new User { Username = "user3", Email = "user3@example.com" };

            // Act
            await _userService.CreateUserAsync(user);

            // Assert
            Assert.Equal(1, _context.Users.Count());
        }

        [Fact]
        public async Task UpdateUserAsync_UpdatesUser()
        {
            // Arrange
            var user = new User { Id = 1, Username = "user1", Email = "user1@example.com" };
            _context.Users.Add(user);
            _context.SaveChanges();
            user.Username = "user1_updated";

            // Act
            await _userService.UpdateUserAsync(user);

            // Assert
            var updatedUser = _context.Users.Find(1);
            Assert.Equal("user1_updated", updatedUser.Username);
        }

        [Fact]
        public async Task DeleteUserAsync_RemovesUser()
        {
            // Arrange
            var user = new User { Id = 1, Username = "user1", Email = "user1@example.com" };
            _context.Users.Add(user);
            _context.SaveChanges();

            // Act
            await _userService.DeleteUserAsync(1);

            // Assert
            Assert.Empty(_context.Users.ToList());
        }
    }
}
