using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Turniejowo.API.Models;
using Xunit;

namespace Turniejowo.API.IntegrationTests.ControllerTests
{
    
    public class MatchControllerTests : IntegrationTest
    {
        [Fact]
        public async Task GetAll_WithoutToken_Returns401()
        {
            var response = await TestClient.GetAsync("api/match");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetAll_WithoutPosts_Returns404()
        {
            //Arrange
            await AuthenticateAsync();

            //Act
            var response = await TestClient.GetAsync("api/match");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound,response.StatusCode);
        }

        [Fact]
        public async Task GetAll_WithPosts_Returns200()
        {
            //Arrange 
            await AuthenticateAsync();

            //Act

            //Assert
        }
    }
}
