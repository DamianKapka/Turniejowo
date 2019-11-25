using AutoMapper;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Turniejowo.API.Helpers.Manager;
using Turniejowo.API.Models;
using Turniejowo.API.Repositories;
using Turniejowo.API.Services;
using Turniejowo.API.UnitOfWork;
using Xunit;

namespace Turniejowo.UnitTests.Service
{
    public class TournamentServiceUnitTests
    {
        private readonly Mock<ITournamentRepository> tournamentRepositoryMock;
        private readonly Mock<ITeamRepository> teamRepositoryMock;
        private readonly Mock<ITeamService> teamServiceMock;
        private readonly Mock<IPlayerRepository> playerRepositoryMock;
        private readonly Mock<IUserRepository> userRepositoryMock;
        private readonly Mock<IMatchRepository> matchRepositoryMock;
        private readonly Mock<IDisciplineRepository> disciplineRepositoryMock;
        private readonly Mock<IPointsRepository> pointsRepositoryMock;
        private readonly IBracketManager bracketManager;
        private readonly Mock<IUnitOfWork> unitOfWorkMock;
        private readonly Mock<IMapper> mapperMock;

        private readonly TournamentService tournamentService;

        public TournamentServiceUnitTests()
        {
            tournamentRepositoryMock = new Mock<ITournamentRepository>();
            teamRepositoryMock = new Mock<ITeamRepository>();
            teamServiceMock = new Mock<ITeamService>();
            playerRepositoryMock = new Mock<IPlayerRepository>();
            userRepositoryMock = new Mock<IUserRepository>();
            matchRepositoryMock = new Mock<IMatchRepository>();
            disciplineRepositoryMock = new Mock<IDisciplineRepository>();
            pointsRepositoryMock = new Mock<IPointsRepository>();
            bracketManager = new BracketManager();
            unitOfWorkMock = new Mock<IUnitOfWork>();
            mapperMock = new Mock<IMapper>();

            tournamentService = new TournamentService(tournamentRepositoryMock.Object, teamRepositoryMock.Object, teamServiceMock.Object, playerRepositoryMock.Object, userRepositoryMock.Object,
                matchRepositoryMock.Object, disciplineRepositoryMock.Object, unitOfWorkMock.Object, pointsRepositoryMock.Object, mapperMock.Object, bracketManager);
        }

        [Fact]
        public async Task GetTournamentBracketAsync_WhenThereAreNoMatchesYet_ShouldFillWithBlanks()
        {
            tournamentRepositoryMock.Setup(t => t.FindSingleAsync(It.IsAny<Expression<Func<Tournament, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(new Tournament
            {
                AmountOfTeams = 16,
                IsBracket = true
            });

            var bracket = await tournamentService.GetTournamentBracketAsync(1);

            bracket.Should().NotBeNull();
        }



    }
}
