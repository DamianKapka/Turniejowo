using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Turniejowo.API.Contracts.Requests;
using Turniejowo.API.Models;
using Xunit;

namespace Turniejowo.API.IntegrationTests.ControllerTests
{
    public class UserControllerTests : IntegrationTest
    {
        #region Get Tests
        [Fact]
        public async Task Get_WithUser_Returns200()
        {
            //Arrange
            await AuthenticateAsync();
            await InsertDummyData();

            //Act 
            var response = await TestClient.GetAsync("api/user/1");


            //Assert
            Assert.Equal(HttpStatusCode.OK,response.StatusCode);
        }

        [Fact]
        public async Task Get_WithoutUser_Returns404()
        {
            //Arrange
            await AuthenticateAsync();
            await InsertDummyData();

            //Act 
            var response = await TestClient.GetAsync("api/user/1338");


            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Get_WithoutToken_Returns401()
        {
            //Act 
            var response = await TestClient.GetAsync("api/user/1");


            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
        #endregion

        #region Register Tests
        [Fact]
        public async Task Register_WithoutToken_DoesNotReturn401()
        {
            //Arrange
            await InsertDummyData();

            //Act 
            var response = await TestClient.PostAsJsonAsync("api/user", new User()
            {
                Email = "damiankapka@live.com",
                FullName = "Damian Kapka",
                Password = "xD",
                Phone = "123-456-789",
            });


            //Assert
            Assert.NotEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Register_InvalidModel_Returns400()
        {
            //Arrange
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.PostAsJsonAsync("api/user", new User()
            {
                FullName = "Damian Kapka",
                Password = "xD",
                Phone = "123-456-789",
            });

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest,response.StatusCode);
        }

        [Fact]
        public async Task Register_ProperModel_Returns201()
        {
            //Arrange
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.PostAsJsonAsync("api/user", new User()
            {
                Email = "damiankapka@live.com",
                FullName = "Damian Kapka",
                Password = "xD",
                Phone = "123-456-789",
            });

            //Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task Register_UserAlreadyExists_Returns409()
        {
            //Arrange
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.PostAsJsonAsync("api/user", new User()
            {
                Email = "test@test.com",
                FullName = "test test",
                Password = "testtest",
                Phone = "123456789",
            });

            //Assert
            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        }
        #endregion

        #region Authenticate Tests
        [Fact]
        public async Task Authenticate_WithoutToken_DoesNotReturn401()
        {
            //Arrange
            await InsertDummyData();

            //Act
            var response = await TestClient.PostAsJsonAsync("api/user/authenticate", new Credentials()
            {
                Login = "test@test.com",
                Password = "testtest",
            });

            //Assert
            Assert.NotEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Authenticate_InvalidCredentials_Retur401()
        {
            //Arrange
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.PostAsJsonAsync("api/user/authenticate", new Credentials()
            {
                Login = "test@test.com",
                Password = "teststest",
            });

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
        #endregion

        #region GetUserTournaments Tests
        [Fact]
        public async Task GetUserTournaments_WithoutToken_DoesNotReturn401()
        {
            //Act
            var response = await TestClient.GetAsync("api/user/1/tournaments");

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetUserTournaments_WithTournaments_Return200()
        {
            //Arrange
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.GetAsync("api/user/1/tournaments");

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetUserTournaments_WithoutTournaments_Return404()
        {
            //Arrange
            await AuthenticateAsync();
            await InsertDummyData();

            //Act
            var response = await TestClient.GetAsync("api/user/133/tournaments");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        #endregion
    }
}
