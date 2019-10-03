using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Turniejowo.API.Contracts.Responses;
using Turniejowo.API.Models;
using Xunit;

namespace Turniejowo.API.IntegrationTests.ControllerTests
{
    public class TeamControllerTests : IntegrationTest
    {
        #region Add Tests
        [Fact]
        public async Task Add_ModelStateInvalid_Returns400()
        {
            //Arrange 
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.PostAsJsonAsync("api/team", new Team()
            {
                Matches = 0,
                Points = 0,
                TournamentId = 1,
                Wins = 0,
            });

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Add_NoTournamentForTeam_Returns404()
        {
            //Arrange 
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.PostAsJsonAsync("api/team", new Team()
            {
                Name = "testTeam",
                Loses = 0,
                Matches = 0,
                Points = 0,
                TournamentId = 3,
                Wins = 0,
            });

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Add_ProperTeam_Returns201()
        {
            //Arrange 
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.PostAsJsonAsync("api/team", new Team()
            {
                Name = "testTeam",
                Loses = 0,
                Matches = 0,
                Points = 0,
                TournamentId = 1,
                Wins = 0,
            });

            //Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task Add_WithoutToken_Returns401()
        {
            //Act
            var response = await TestClient.PostAsJsonAsync("api/team", new Team()
            {
                Name = "testTeam",
                Loses = 0,
                Matches = 0,
                Points = 0,
                TournamentId = 2,
                Wins = 0,
            });

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Add_SameTeamInTheTournament_Returns409()
        {
            //Arrange 
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.PostAsJsonAsync("api/team", new Team()
            {
                TournamentId = 1,
                Name = "testteam1",
                Points = 0,
                Wins = 0,
                Loses = 0,
                Matches = 0,
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
            var response = await TestClient.DeleteAsync("/api/team/4");

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
            var response = await TestClient.DeleteAsync("/api/team/1");

            //Assert
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
        }

        [Fact]
        public async Task Delete_WithoutToken_Returns401()
        {
            //Arrange

            //Act
            var response = await TestClient.DeleteAsync("/api/team/1");

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
        #endregion

        #region Edit Tests

        [Fact]
        public async Task Edit_MismatchedIdAndTeamId_Returns409()
        {
            //Arrange
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.PutAsJsonAsync("api/team/1", new Team()
            {
                TeamId = 2,
                TournamentId = 1,
                Name = "testteam1",
                Points = 0,
                Wins = 0,
                Loses = 0,
                Matches = 0,
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
            var response = await TestClient.PutAsJsonAsync("api/team/1", new Team()
            {
                TournamentId = 1,
                Points = 0,
                Wins = 0,
                Loses = 0,
                Matches = 0,
            });

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Edit_NoTournamentForTeam_Returns404()
        {
            //Arrange
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.PutAsJsonAsync("api/team/1", new Team()
            {
                TeamId = 1,
                TournamentId = 3,
                Name = "testteam1",
                Points = 0,
                Wins = 0,
                Loses = 0,
                Matches = 0,
            });

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Edit_ProperTeam_Returns202()
        {
            //Arrange
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.PutAsJsonAsync("api/team/1", new Team()
            {
                TeamId = 1,
                TournamentId = 1,
                Name = "testteam1",
                Points = 0,
                Wins = 0,
                Loses = 0,
                Matches = 0,
            });

            //Assert
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
        }

        [Fact]
        public async Task Edit_WithoutToken_Returns401()
        {
            //Arrange

            //Act
            var response = await TestClient.PutAsJsonAsync("api/team/1", new Team()
            {
                TeamId = 2,
                TournamentId = 1,
                Name = "testteam1",
                Points = 0,
                Wins = 0,
                Loses = 0,
                Matches = 0,
            });

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
        #endregion

        #region Get Tests
        [Fact]
        public async Task Get_WithOneTeam_Returns200()
        {
            //Arrange 
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.GetAsync("api/team/1");
            var responseContent = await response.Content.ReadAsAsync<TeamResponse>();

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(responseContent);
        }

        [Fact]
        public async Task Get_WithOutAuth_Returns401()
        {
            //Arrange

            //Act
            var response = await TestClient.GetAsync("api/team/1");

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Get_WithTeam_Returns200()
        {
            //Arrange 
            await AuthenticateAsync();
            await InsertDummyData();


            //Act 
            var response = await TestClient.GetAsync("api/team/1");

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Get_WithWrongId_Returns404()
        {
            //Arrange 
            await AuthenticateAsync();
            await InsertDummyData();


            //Act 
            var response = await TestClient.GetAsync("api/team/5");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        #endregion

        #region GetPlayerForTeam Tests

        [Fact]
        public async Task GetPlayersForTeam_WithoutToken_DoesNotReturn401()
        {
            //Arrange
            await InsertDummyData();

            //Act
            var response = await TestClient.GetAsync("api/team/1/players");

            //Assert
            Assert.NotEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetPlayersForTeam_WithTeams_Returns200()
        {
            //Arrange 
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.GetAsync("api/team/1/players");

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetPlayersForTeam_WithoutTeams_Returns404()
        {
            //Arrange 
            await AuthenticateAsync();

            //Act
            var response = await TestClient.GetAsync("api/team/1/players");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetPlayersForTeam_WithoutPlayers_Returns200()
        {
            //Arrange
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.GetAsync("api/team/3/players");

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        #endregion

        #region GetMatchesForTeam Tests

        [Fact]
        public async Task GetMatchesForTeam_WithoutToken_DoesNotReturn401()
        {
            //Arrange
            await InsertDummyData();

            //Act
            var response = await TestClient.GetAsync("api/team/1/matches");

            //Assert
            Assert.NotEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetMatchesForTeam_WithTeams_Returns200()
        {
            //Arrange 
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.GetAsync("api/team/1/matches");

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetMatchesForTeam_WithoutTeams_Returns404()
        {
            //Arrange 
            await AuthenticateAsync();

            //Act
            var response = await TestClient.GetAsync("api/team/1/matches");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetMatchesForTeam_WithoutMatches_Returns200()
        {
            //Arrange
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.GetAsync("api/team/3/matches");

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        #endregion
    }
}