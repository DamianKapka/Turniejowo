using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace Turniejowo.API.IntegrationTests
{
    public class TournamentIntegrationTests
    {
        private readonly HttpClient _client;
        private readonly TestServer _server;

        public TournamentIntegrationTests()
        {
            _server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            _client = _server.CreateClient(); 
        }

        [Fact]
        public async Task TestTournamentsGet()
        {
            //ASSERT
            var request = new HttpRequestMessage(new HttpMethod("GET"),"/api/tournament/1");

            //ACT
            var response = await _client.SendAsync(request);

            //ARRANGE
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK,response.StatusCode);
        }

    }
}
