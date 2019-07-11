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

        }
    }
}
