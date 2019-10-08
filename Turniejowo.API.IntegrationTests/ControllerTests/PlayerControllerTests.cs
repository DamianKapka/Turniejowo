using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Turniejowo.API.Contracts.Responses;
using Turniejowo.API.Models;
using Xunit;

namespace Turniejowo.API.IntegrationTests.ControllerTests
{
    public class PlayerControllerTests : IntegrationTest
    {
        #region Add Tests
        [Fact]
        public async Task Add_ModelStateInvalid_Returns400()
        {
            //Arrange 
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.PostAsJsonAsync("api/player", new Player()
            {
                LName = "Kapka",
                TeamId = 1,
            });

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Add_NoTeamForPlayer_Returns404()
        {
            //Arrange 
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.PostAsJsonAsync("api/player", new Player()
            {
                FName = "Damian",
                LName = "Kapka",
                TeamId = 4,
            });

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Add_ProperPlayer_Returns201()
        {
            //Arrange 
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.PostAsJsonAsync("api/player", new Player()
            {
                FName = "Damian",
                LName = "Kapka",
                TeamId = 1,
            });

            //Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task Add_WithoutToken_Returns401()
        {
            //Act
            var response = await TestClient.PostAsJsonAsync("api/player", new Player()
            {
                FName = "Damian",
                LName = "Kapka",
                TeamId = 1,
            });

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Add_SamePlayerInTheTeam_Returns409()
        {
            //Arrange 
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.PostAsJsonAsync("api/player", new Player()
            {
                FName = "testPlayerrr",
                LName = "testPlayerrr",
                TeamId = 1,
            });

            //Assert
            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        }

        #endregion

        #region Delete Tests
        [Fact]
        public async Task Delete_NonExistingId_Returns404()
        {
            //Arrange
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.DeleteAsync("/api/player/3");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_ProperId_Returns202()
        {
            //Arrange
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.DeleteAsync("/api/player/1");

            //Assert
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
        }

        [Fact]
        public async Task Delete_WithoutToken_Returns401()
        { 
            //Arrange

            //Act
            var response = await TestClient.DeleteAsync("/api/player/1");

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
        #endregion

        #region Edit Tests

        [Fact]
        public async Task Edit_MismatchedIdAndPlayerId_Returns409()
        {
            //Arrange
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.PutAsJsonAsync("api/player/1", new Player()
            {
                PlayerId = 3,
                FName = "Damian",
                LName = "Kapka",
                TeamId = 1
            });

            //Assert
            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        }

        [Fact]
        public async Task Edit_ModelStateInvalid_Returns400()
        {
            //Arrange
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.PutAsJsonAsync("api/player/1", new Player()
            {
                PlayerId = 1,
                FName = "Damian",
                TeamId = 1
            });

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Edit_NoTeamForPlayer_Returns404()
        {
            //Arrange
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.PutAsJsonAsync("api/player/1", new Player()
            {
                PlayerId = 1,
                FName = "Damian",
                LName = "Kapka",
                TeamId = 1337
            });

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Edit_ProperPlayer_Returns202()
        {
            //Arrange
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.PutAsJsonAsync("api/player/1", new Player()
            {
                PlayerId = 1,
                FName = "Damian",
                LName = "Kapka",
                TeamId = 1
            });

            //Assert
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
        }

        [Fact]
        public async Task Edit_WithoutToken_Returns401()
        {
            //Arrange

            //Act
            var response = await TestClient.PutAsJsonAsync("api/player/1", new Player()
            {
                PlayerId = 1,
                FName = "Damian",
                LName = "Kapka",
                TeamId = 1
            });

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
        #endregion

        #region Get Tests
        [Fact]
        public async Task Get_WithOnePlayer_Returns200()
        {
            //Arrange 
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.GetAsync("api/player/1");
            var responseContent = await response.Content.ReadAsAsync<PlayerResponse>();

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(responseContent);
        }

        [Fact]
        public async Task Get_WithOutAuth_Returns401()
        {
            //Arrange

            //Act
            var response = await TestClient.GetAsync("api/player/1");

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized,response.StatusCode);
        }

        [Fact]
        public async Task Get_WithPlayer_Returns200()
        {
            //Arrange 
            await AuthenticateAsync();
            await InsertDummyData();


            //Act 
            var response = await TestClient.GetAsync("api/player/1");

            //Assert
            Assert.Equal(HttpStatusCode.OK,response.StatusCode);
        }

        [Fact]
        public async Task Get_WithWrongId_Returns404()
        {
            //Arrange 
            await AuthenticateAsync();
            await InsertDummyData();


            //Act 
            var response = await TestClient.GetAsync("api/player/5");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        #endregion
    }
}
