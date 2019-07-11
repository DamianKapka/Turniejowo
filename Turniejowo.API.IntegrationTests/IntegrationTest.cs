﻿using System;
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
                DisciplineId = 3,
                EntryFee = 20,
                Localization = "testLocalization"
            });

            var tmResponse = await TestClient.PostAsJsonAsync("api/team", new Team()
            {
                TournamentId = 1,
                Name = "testteam1",
                Points = 0,
                Wins = 0,
                Loses = 0,
                Matches = 0,
            });

            var tm2Response = await TestClient.PostAsJsonAsync("api/team", new Team()
            {
                TournamentId = 1,
                Name = "testteam2",
                Points = 0,
                Wins = 0,
                Loses = 0,
                Matches = 0
            });

            var plResponse = await TestClient.PostAsJsonAsync("api/player", new Player()
            {
                FName = "testPlayerrr",
                LName = "testPlayerrr",
                TeamId = 1,
                Points = 0,
            });

            var pl2Response = await TestClient.PostAsJsonAsync("api/player", new Player()
            {
                FName = "testPlayerr",
                LName = "testPlayerr",
                TeamId = 2,
                Points = 0,
            });

            var mtResponse = await TestClient.PostAsJsonAsync("api/match", new Match()
            {
                HomeTeamId = 1,
                GuestTeamId = 2,
                GuestTeamPoints = 3,
                HomeTeamPoints = 2,
            });
        }
    }
}