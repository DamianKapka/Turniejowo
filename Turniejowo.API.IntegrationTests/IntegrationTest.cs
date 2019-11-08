using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Turniejowo.API.Contracts.Requests;
using Turniejowo.API.Models;
using Turniejowo.API.Repositories;

namespace Turniejowo.API.IntegrationTests
{
    public class IntegrationTest
    {
        protected readonly HttpClient TestClient;
        protected bool IsTestUserRegistered;

        protected IntegrationTest()
        {
            var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder => { builder.ConfigureServices(services =>
                    {
                        services.RemoveAll(typeof(TurniejowoDbContext));
                        services.AddDbContext<TurniejowoDbContext>(options =>
                        {
                            options.UseInMemoryDatabase("TestDb");  

                        });
                    });
            });
            TestClient = appFactory.CreateClient();
        }

        protected async Task AuthenticateAsync()
        {
            TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer",await GetJwtAsync());
        }

        private async Task<string> GetJwtAsync()
        {
            if (!IsTestUserRegistered)
            {
                var lolo = await TestClient.PostAsJsonAsync("api/user", new User()
                {
                    Email = "test@test.com",
                    FullName = "test test",
                    Password = "testtest",
                    Phone = "123456789",
                });

                IsTestUserRegistered = true; 
            }

            var response = await TestClient.PostAsJsonAsync("api/user/authenticate", new Credentials()
            {
                Login = "test@test.com",
                Password = "testtest"
            });

            var authResponse = await response.Content.ReadAsAsync<User>();

            return authResponse.Token;
        }

        protected async Task InsertDummyData()
        {
            var dsResponse = await TestClient.PostAsJsonAsync("api/discipline", new Discipline()
            {
                DisciplineId = 1,
                Name = "Zulerka"
            });

            var usrResponse = await TestClient.PostAsJsonAsync("api/user", new User()
            {
                Email = "test2@test.com",
                FullName = "test test",
                Password = "testPw",
                Phone = "123456789",
            });

            var trnResponse = await TestClient.PostAsJsonAsync("api/tournament", new Tournament()
            {
                Name = "testTourney",
                AmountOfTeams = 1,
                CreatorId = 1,
                Date = DateTime.Now,
                DisciplineId = 1,
                EntryFee = 20,
                Localization = "testLocalization"
            });

            var trn2Response = await TestClient.PostAsJsonAsync("api/tournament", new Tournament()
            {
                Name = "testTourneyNext",
                AmountOfTeams = 1,
                CreatorId = 1,
                Date = DateTime.Now,
                DisciplineId = 1,
                EntryFee = 20,
                Localization = "testLocalization"
            });

            var tmResponse = await TestClient.PostAsJsonAsync("api/team", new Team()
            {
                TeamId = 1,
                TournamentId = 1,
                Name = "testteam1",
                Points = 0,
                Wins = 0,
                Loses = 0,
                Matches = 0,
            });

            var tm2Response = await TestClient.PostAsJsonAsync("api/team", new Team()
            {
                TeamId = 2,
                TournamentId = 1,
                Name = "testteam2",
                Points = 0,
                Wins = 0,
                Loses = 0,
                Matches = 0
            });

            var tm3Response = await TestClient.PostAsJsonAsync("api/team", new Team()
            {
                TeamId = 3,
                TournamentId = 1,
                Name = "testteam3",
                Points = 0,
                Wins = 0,
                Loses = 0,
                Matches = 0
            });

            var plResponse = await TestClient.PostAsJsonAsync("api/player", new Player()
            {
                PlayerId = 1,
                FName = "testPlayerrr",
                LName = "testPlayerrr",
                TeamId = 1,
            });

            var pl2Response = await TestClient.PostAsJsonAsync("api/player", new Player()
            {
                PlayerId = 2,
                FName = "testPlayerr",
                LName = "testPlayerr",
                TeamId = 2,
            });

            var pl3Response = await TestClient.PostAsJsonAsync("api/player", new Player()
            {
                PlayerId = 3,
                FName = "testPlayerrrrrr",
                LName = "testPlayerrrrrr",
                TeamId = 3,
            });

            var pl6Response = await TestClient.PostAsJsonAsync("api/player", new Player()
            {
                PlayerId = 6,
                FName = "testPlayerrrooooo",
                LName = "testPlayerrrrrr",
                TeamId = 1,
            });

            var pl7Response = await TestClient.PostAsJsonAsync("api/player", new Player()
            {
                PlayerId = 7,
                FName = "testPlayerrrooooo",
                LName = "testPlayerrrrrr",
                TeamId = 2,
            });

            var mtResponse = await TestClient.PostAsJsonAsync("api/match", new Match()
            {
                MatchId = 1,
                HomeTeamId = 1,
                GuestTeamId = 2,
                HomeTeamPoints = 3,
                GuestTeamPoints = 2,
                MatchDateTime = new DateTime(2012,9,11,12,00,00)
            });

            var mtResponse2 = await TestClient.PostAsJsonAsync("api/match", new Match()
            {
                MatchId = 2,
                HomeTeamId = 2,
                GuestTeamId = 1,
                GuestTeamPoints = 1,
                HomeTeamPoints = 1,
                MatchDateTime = new DateTime(2012, 9, 11, 14, 00, 00)
            });

            var ptResponse = await TestClient.PostAsJsonAsync("api/points",
                new Points()
                {
                    PointsId = 1,
                    MatchId = 1,
                    PlayerId = 1,
                    PointsQty = 2,
                    TournamentId = 1,
                });

            var ptResponse2 = await TestClient.PostAsJsonAsync("api/points",
                new Points()
                {
                    PointsId = 2,
                    MatchId = 1,
                    PlayerId = 2,
                    PointsQty = 2,
                    TournamentId = 1,
                });
        }
    }
}
