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
        private readonly Match testMatch = new Match()
        {
            GuestTeamId = 1,
            HomeTeamId = 2,
            HomeTeamPoints = 20,
            GuestTeamPoints = 30
        };

        #region GetAll Tests
        [Fact]
        public async Task GetAll_WithoutToken_Returns401()
        {
            var response = await TestClient.GetAsync("api/match");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetAll_WithoutMatches_Returns404()
        {
            //Arrange
            await AuthenticateAsync();

            //Act
            var response = await TestClient.GetAsync("api/match");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetAll_WithMatches_Returns200()
        {
            //Arrange 
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.GetAsync("api/match");

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetAll_WithOneMatch_ReturnSizeOneArray()
        {
            //Arrange 
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.GetAsync("api/match");
            var responseContent = await response.Content.ReadAsAsync<Match[]>();

            //Assert
            Assert.Single(responseContent);
        }
        #endregion

        #region Get Tests
        [Fact]
        public async Task Get_WithoutPosts_Returns404()
        {
            //Arrange
            await AuthenticateAsync();

            //Act
            var response = await TestClient.GetAsync("/api/match/1");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound,response.StatusCode);
        }

        [Fact]
        public async Task Get_WithoutAuth_DoesNotReturns_401()
        {
            //Arrange

            //Act
            var response = await TestClient.GetAsync("/api/match/1");

            //Assert
            Assert.NotEqual(HttpStatusCode.Unauthorized,response.StatusCode);
        }

        [Fact]
        public async Task Get_WithOneMatch_Returns200()
        {
            //Arrange 
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.GetAsync("api/match/1");
            var responseContent = await response.Content.ReadAsAsync<Match>();

            //Assert
            Assert.Equal(HttpStatusCode.OK,response.StatusCode);
            Assert.NotNull(responseContent);
        }
        #endregion

        #region Add Tests
        [Fact]
        public async Task Add_WithoutToken_Returns401()
        {
            //Arrange

            //Act
            var response = await TestClient.PostAsJsonAsync("api/match", testMatch);

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized,response.StatusCode);
        }

        [Fact]
        public async Task Add_ModelStateInvalid_Returns400()
        {
            //Arrange 
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.PostAsJsonAsync("api/match", new Match()
            {
                HomeTeamId = 2,
                HomeTeamPoints = 20,
                GuestTeamPoints = 30
            });

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Add_ProperMatch_Returns201()
        {
            //Arrange 
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.PostAsJsonAsync("api/match", testMatch);

            //Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task Add_NoTeamForMatch_Returns404()
        {
            //Arrange 
            await AuthenticateAsync();

            //Act
            var response = await TestClient.PostAsJsonAsync("api/match", new Match()
            {
                GuestTeamId = 1337,
                HomeTeamId = 2,
                HomeTeamPoints = 20,
                GuestTeamPoints = 30
            });

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        #endregion

        #region Edit Tests
        [Fact]
        public async Task Edit_WithoutToken_Returns_401()
        {
            //Arrange

            //Act
            var response = await TestClient.PutAsJsonAsync("api/match/1", testMatch);

            //Arrange
            Assert.Equal(HttpStatusCode.Unauthorized,response.StatusCode);
        }

        [Fact]
        public async Task Edit_ModelStateInvalid_Returns_400()
        {
            //Arrange
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.PutAsJsonAsync("api/match/1", new Match()
            {
                HomeTeamId = 2,
                HomeTeamPoints = 20,
                GuestTeamPoints = 30
            });

            //Arrange
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Edit_ProperMatch_Returns202()
        {
            //Arrange
            await AuthenticateAsync();
            await InsertDummyData();

            //Act


            var response = await TestClient.PutAsJsonAsync("api/match/1", new Match()
            {
                MatchId = 1,
                GuestTeamId = 1,
                HomeTeamId = 2,
                HomeTeamPoints = 20,
                GuestTeamPoints = 30
            });

            //Assert
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
        }

        [Fact]
        public async Task Edit_NoTeamForMatch_Returns404()
        {
            //Arrange
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.PutAsJsonAsync("api/match/1", new Match()
            {
                MatchId = 1,
                GuestTeamId = 1337,
                HomeTeamId = 2,
                HomeTeamPoints = 20,
                GuestTeamPoints = 30
            });

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Edit_MismatchedIdAndMatchId_Returns409()
        {
            //Arrange
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.PutAsJsonAsync("api/match/1", new Match()
            {
                MatchId = 2,
                GuestTeamId = 1,
                HomeTeamId = 2,
                HomeTeamPoints = 20,
                GuestTeamPoints = 30
            });

            //Assert
            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        }
        #endregion

        #region Delete Tests

        [Fact]
        public async Task Delete_WithoutToken_Returns401()
        {
            //Arrange

            //Act
            var response = await TestClient.DeleteAsync("/api/match/1");

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Delete_ProperId_Returns202()
        {
            //Arrange
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.DeleteAsync("/api/match/1");

            //Assert
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
        }

        [Fact]
        public async Task Delete_NotExistingId_Returns404()
        {
            //Arrange
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.DeleteAsync("/api/match/3");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }


        #endregion
    }
}
