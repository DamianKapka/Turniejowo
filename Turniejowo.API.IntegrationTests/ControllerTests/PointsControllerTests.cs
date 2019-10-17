using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Turniejowo.API.Models;
using Xunit;

namespace Turniejowo.API.IntegrationTests.ControllerTests
{ 
    public class PointsControllerTests : IntegrationTest
    {
        #region AddPoints Tests

        [Fact]
        public async Task AddPoints_NoToken_Returns401()
        {
            //Arrange 
            await InsertDummyData();

            //Act
            var response = await TestClient.PostAsJsonAsync("api/points", new List<Points>
            {
                new Points()
                {
                    PointsId = 3,
                    MatchId = 1,
                    PlayerId = 1,
                    PointsQty = 3,
                }
            });

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized,response.StatusCode);
        }

        [Fact]
        public async Task AddPoints_NotArray_Returns400()
        {
            //Arrange 
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.PostAsJsonAsync("api/points", new Points()
                {
                    PointsId = 3,
                    MatchId = 1,
                    PlayerId = 1,
                    PointsQty = 3,
                }
            );

                //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task AddPoints_ValidRequest_Returns202()
        {
            //Arrange 
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.PostAsJsonAsync("api/points", new List<Points>
            {
                new Points()
                {
                    PointsId = 3,
                    MatchId = 1,
                    PlayerId = 1,
                    PointsQty = 3,
                }
            });

            //Assert
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
        }

        [Fact]
        public async Task AddPoints_PlayerNotFromParticipantTeam_Returns409()
        {
            //Arrange 
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.PostAsJsonAsync("api/points", new List<Points>
            {
                new Points()
                {
                    PointsId = 3,
                    MatchId = 1,
                    PlayerId = 3,
                    PointsQty = 3,
                }
            });

            //Assert
            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        }

        [Fact]
        public async Task AddPoints_Add2ProperEntries_ReturnsCounts3()
        {
            //Arrange 
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.PostAsJsonAsync("api/points", new List<Points>
            {
                new Points()
                {
                    PointsId = 3,
                    MatchId = 1,
                    PlayerId = 1,
                    PointsQty = 3,
                    TournamentId = 1,
                },
                new Points()
                {
                    PointsId = 4,
                    MatchId = 1,
                    PlayerId = 2,
                    PointsQty = 2,
                    TournamentId = 1,
                }
            });

            var points = await TestClient.GetAsync("api/tournament/1/points");
            var pointsContent = await points.Content.ReadAsAsync<List<Points>>();

            //Assert
            Assert.Equal(4,pointsContent.Count);
        }
        #endregion

        #region EditPoints Test
        [Fact]
        public async Task EditPoints_NoToken_Returns401()
        {
            //Arrange
            await InsertDummyData();

            //Act
            var response = await TestClient.PutAsJsonAsync("api/points/1", new List<Points>
            {
                new Points()
                {
                    MatchId = 1,
                    PlayerId = 1,
                    PointsId = 1,
                    PointsQty = 3,
                    TournamentId = 1
                }
            });

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized,response.StatusCode);
        }

        [Fact]
        public async Task EditPoints_NotArray_Returns400()
        {
            //Arrange
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.PutAsJsonAsync("api/points/1", new Points()
                {
                    MatchId = 1,
                    PlayerId = 1,
                    PointsId = 1,
                    PointsQty = 3,
                    TournamentId = 1
                });

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task EditPoints_ProperRequest_Returns202()
        {
            //Arrange
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.PutAsJsonAsync("api/points/1", new List<Points>
            {
                new Points()
                {
                    MatchId = 1,
                    PlayerId = 1,
                    PointsId = 1,
                    PointsQty = 3,
                    TournamentId = 1
                }
            });

            //Assert
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
        }

        [Fact]
        public async Task EditPoints_PlayerNotParticipant_Returns409()
        {
            //Arrange
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.PutAsJsonAsync("api/points/1", new List<Points>
            {
                new Points()
                {
                    MatchId = 1,
                    PlayerId = 3,
                    PointsId = 1,
                    PointsQty = 3,
                    TournamentId = 1
                }
            });

            //Assert
            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        }
        #endregion

        #region DeletePoints Tests

        [Fact]
        public async Task DeletePoints_NoToken_Returns401()
        {
            //Arrange
            await InsertDummyData();

            //Act
            var response = await TestClient.DeleteAsync("api/points/1");

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized,response.StatusCode);
        }

        [Fact]
        public async Task DeletePoints_NoMatch_Returns404()
        {
            //Arrange
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.DeleteAsync("api/points/5");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeletePoints_NoPointsForMatch_Returns404()
        {
            //Arrange
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.DeleteAsync("api/points/3");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeletePoints_ValidRequest_Returns202()
        {
            //Arrange
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.DeleteAsync("api/points/1");

            //Assert
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
        }
        #endregion
    }
}
