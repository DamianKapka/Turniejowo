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
    public class TournamentControllerTests : IntegrationTest
    {
        #region Add Tests
        [Fact]
        public async Task Add_ModelStateInvalid_Returns400()
        {
            //Arrange 
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.PostAsJsonAsync("api/tournament", new Tournament()
            {
                Name = "test",
                AmountOfTeams = 5,
                CreatorId = 1,
                Date = DateTime.Now,
                DisciplineId = 3,
            });

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Add_NoUserForTournament_Returns404()
        {
            //Arrange 
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.PostAsJsonAsync("api/tournament", new Tournament()
            {
                Name = "test",
                AmountOfTeams = 5,
                CreatorId = 1337,
                Date = DateTime.Now,
                DisciplineId = 3,
                EntryFee = 0,
                Localization = "testLocalization 11/11",
            });

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Add_ProperTournament_Returns201()
        {
            //Arrange 
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.PostAsJsonAsync("api/tournament", new Tournament()
            {
                Name = "test",
                AmountOfTeams = 5,
                CreatorId = 1,
                Date = DateTime.Now,
                DisciplineId = 3,
                EntryFee = 0,
                Localization = "testLocalization 11/11",
            });

            //Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task Add_WithoutToken_Returns401()
        {
            //Arrange 

            //Act
            var response = await TestClient.PostAsJsonAsync("api/tournament", new Tournament()
            {
                Name = "test",
                AmountOfTeams = 5,
                CreatorId = 2,
                Date = DateTime.Now,
                DisciplineId = 3,
                EntryFee = 0,
                Localization = "testLocalization 11/11",
            });

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Add_SameTournamentForUser_Returns409()
        {
            //Arrange 
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.PostAsJsonAsync("api/tournament", new Tournament()
            {
                Name = "testTourney",
                AmountOfTeams = 1,
                CreatorId = 1,
                Date = DateTime.Now,
                DisciplineId = 3,
                EntryFee = 20,
                Localization = "testLocalization"
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
            var response = await TestClient.DeleteAsync("/api/tournament/3");

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
            var response = await TestClient.DeleteAsync("/api/tournament/1");

            //Assert
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
        }

        [Fact]
        public async Task Delete_WithoutToken_Returns401()
        {
            //Arrange

            //Act
            var response = await TestClient.DeleteAsync("/api/tournament/1");

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
            var response = await TestClient.PutAsJsonAsync("api/tournament/1", new Tournament()
            {
                TournamentId = 2,
                Name = "testTourney",
                AmountOfTeams = 1,
                CreatorId = 1,
                Date = DateTime.Now,
                DisciplineId = 3,
                EntryFee = 20,
                Localization = "testLocalization"
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
            var response = await TestClient.PutAsJsonAsync("api/tournament/1", new Tournament()
            {
                TournamentId = 1,
                AmountOfTeams = 1,
                CreatorId = 1,
                Date = DateTime.Now,
                DisciplineId = 3,
                EntryFee = 20,
                Localization = "testLocalization"
            });

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Edit_NoUserForTournament_Returns404()
        {
            //Arrange
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.PutAsJsonAsync("api/tournament/1", new Tournament()
            {
                TournamentId = 1,
                Name = "testTourney",
                AmountOfTeams = 1,
                CreatorId = 3,
                Date = DateTime.Now,
                DisciplineId = 3,
                EntryFee = 20,
                Localization = "testLocalization"
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
            var response = await TestClient.PutAsJsonAsync("api/tournament/1", new Tournament()
            {
                TournamentId = 1,
                Name = "testTourney",
                AmountOfTeams = 1,
                CreatorId = 1,
                Date = DateTime.Now,
                DisciplineId = 3,
                EntryFee = 20,
                Localization = "testLocalization"
            });

            //Assert
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
        }

        [Fact]
        public async Task Edit_WithoutToken_Returns401()
        {
            //Arrange

            //Act
            var response = await TestClient.PutAsJsonAsync("api/tournament/1", new Tournament()
            {
                TournamentId = 2,
                Name = "testTourney",
                AmountOfTeams = 1,
                CreatorId = 1,
                Date = DateTime.Now,
                DisciplineId = 3,
                EntryFee = 20,
                Localization = "testLocalization"
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
            var response = await TestClient.GetAsync("api/tournament/1");
            var responseContent = await response.Content.ReadAsAsync<Tournament>();

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(responseContent);
        }

        [Fact]
        public async Task Get_WithOutAuth_DoesNotReturn401()
        {
            //Arrange

            //Act
            var response = await TestClient.GetAsync("api/tournament/1");

            //Assert
            Assert.NotEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Get_WithTournament_Returns200()
        {
            //Arrange 
            await AuthenticateAsync();
            await InsertDummyData();


            //Act 
            var response = await TestClient.GetAsync("api/tournament/1");

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
            var response = await TestClient.GetAsync("api/tournament/5");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        #endregion

        #region GetTeamsForTournament Tests

        [Fact]
        public async Task GetTeamsForTournament_WithoutToken_DoesNotReturn401()
        {
            //Arrange
            await InsertDummyData();

            //Act
            var response = await TestClient.GetAsync("api/tournament/1/teams");

            //Assert
            Assert.NotEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetTeamsForTournament_WithTeams_Returns200()
        {
            //Arrange 
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.GetAsync("api/tournament/1/teams");

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetPlayersForTeam_WithoutTournaments_Returns404()
        {
            //Arrange 
            await AuthenticateAsync();

            //Act
            var response = await TestClient.GetAsync("api/tournament/1/teams");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        #endregion

        #region GetPlayersForTournament Tests

        [Fact]
        public async Task GetPlayersForTournament_WithoutToken_DoesNotReturn401()
        {
            //Arrange
            await InsertDummyData();

            //Act
            var response = await TestClient.GetAsync("api/tournament/1/players");

            //Assert
            Assert.NotEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetPlayersForTournamentWithPlayers_Returns200()
        {
            //Arrange 
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.GetAsync("api/tournament/1/players");

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetPlayersForTournament_WithoutPlayers_Returns404()
        {
            //Arrange 
            await AuthenticateAsync();

            //Act
            var response = await TestClient.GetAsync("api/tournament/1/players");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        #endregion

        #region GetMatchesForTournament Tests

        [Fact]
        public async Task GetMatchesForTournament_WithoutToken_DoesNotReturn401()
        {
            //Arrange
            await InsertDummyData();

            //Act
            var response = await TestClient.GetAsync("/api/tournament/1/matches");

            //Assert
            Assert.NotEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetMatchesForTournament_WithoutMatches_Returns404()
        {
            //Arrange
            await AuthenticateAsync();

            //Act
            var response = await TestClient.GetAsync("/api/tournament/1/matches");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        #endregion
    }
}
