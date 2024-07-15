using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UserManagementAPI.Models;
using Xunit;

namespace UserManagementAPI.Tests.IntegrationTests
{
    public class UserControllerIntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public UserControllerIntegrationTests(WebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetUsers_ReturnsOkResult()
        {
            // Arrange
            var response = await _client.GetAsync("/api/users");

            // Act
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<List<User>>(responseString);

            // Assert
            Assert.NotEmpty(users);
        }

        [Fact]
        public async Task GetUserById_ReturnsOkResult()
        {
            // Arrange
            var response = await _client.GetAsync("/api/users/1");

            // Act
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<User>(responseString);

            // Assert
            Assert.Equal(1, user.Id);
        }

        [Fact]
        public async Task CreateUser_ReturnsCreatedResult()
        {
            // Arrange
            var newUser = new User { Username = "user4", Email = "user4@example.com" };
            var content = new StringContent(JsonConvert.SerializeObject(newUser), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/users", content);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<User>(responseString);
            Assert.Equal("user4", user.Username);
        }

        [Fact]
        public async Task UpdateUser_ReturnsOkResult()
        {
            // Arrange
            var updatedUser = new User { Id = 1, Username = "user1_updated", Email = "user1_updated@example.com" };
            var content = new StringContent(JsonConvert.SerializeObject(updatedUser), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync("/api/users/1", content);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<User>(responseString);
            Assert.Equal("user1_updated", user.Username);
        }

        [Fact]
        public async Task DeleteUser_ReturnsNoContentResult()
        {
            // Act
            var response = await _client.DeleteAsync("/api/users/1");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
