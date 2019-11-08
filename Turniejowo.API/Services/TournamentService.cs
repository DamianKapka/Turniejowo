using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Turniejowo.API.Contracts.Responses;
using Turniejowo.API.Exceptions;
using Turniejowo.API.MappingProfiles;
using Turniejowo.API.Models;
using Turniejowo.API.Repositories;
using Turniejowo.API.UnitOfWork;

namespace Turniejowo.API.Services
{
    public class TournamentService : ITournamentService
    {
        private readonly ITournamentRepository tournamentRepository;
        private readonly ITeamRepository teamRepository;
        private readonly ITeamService teamService;
        private readonly IPlayerRepository playerRepository;
        private readonly IUserRepository userRepository;
        private readonly IMatchRepository matchRepository;
        private readonly IDisciplineRepository disciplineRepository;
        private readonly IPointsRepository pointsRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public TournamentService(ITournamentRepository tournamentRepository, ITeamRepository teamRepository, ITeamService teamService, IPlayerRepository playerRepository, IUserRepository userRepository, IMatchRepository matchRepository, IDisciplineRepository disciplineRepository, IUnitOfWork unitOfWork, IPointsRepository pointsRepository, IMapper mapper)
        {
            this.tournamentRepository = tournamentRepository;
            this.teamRepository = teamRepository;
            this.teamService = teamService;
            this.playerRepository = playerRepository;
            this.userRepository = userRepository;
            this.matchRepository = matchRepository;
            this.disciplineRepository = disciplineRepository;
            this.pointsRepository = pointsRepository;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<Tournament> GetTournamentByIdAsync(int id)
        {
            var tournament = await tournamentRepository.FindSingleAsync(t => t.TournamentId == id,new string[]{"Discipline", "Creator"})
                             ?? throw new NotFoundInDatabaseException();

            return tournament;
        }

        public async Task<Table> GetTournamentTable(int id)
        {
            var tournamentTeams = await GetTournamentTeamsAsync(id) ?? throw new NotFoundInDatabaseException();
            var tournamentMatches = await GetTournamentMatchesAsync(id);

            foreach (var match in tournamentMatches.Where(t => t.IsFinished))
            {
                Team winner;
                Team loser;

                if (match.HomeTeamPoints > match.GuestTeamPoints)
                {
                    winner = tournamentTeams.First(x => x.TeamId == match.HomeTeamId);

                    winner.Wins++;
                    winner.Points += 3;
                    winner.Matches++;

                    loser = tournamentTeams.First(x => x.TeamId == match.GuestTeamId);
                    loser.Matches++;
                    loser.Loses++;

                }
                else if (match.HomeTeamPoints == match.GuestTeamPoints)
                {
                    winner = tournamentTeams.First(x => x.TeamId == match.HomeTeamId);
                    winner.Points++;
                    winner.Draws++;
                    winner.Matches++;

                    loser = tournamentTeams.First(x => x.TeamId == match.GuestTeamId);
                    loser.Points++;
                    loser.Draws++;
                    loser.Matches++;
                }
                else
                {
                    winner = tournamentTeams.First(x => x.TeamId == match.GuestTeamId);
                    winner.Points += 3;
                    winner.Wins++;
                    winner.Matches++;

                    loser = tournamentTeams.First(x => x.TeamId == match.HomeTeamId);
                    loser.Loses++;
                    loser.Matches++;
                }
            }

            var tableEntries = tournamentTeams.Select(team => mapper.Map<TableEntry>(team)).ToList().OrderByDescending(t => int.Parse(t.Points));

            return new Table()
            {
                TableData = tableEntries.ToList()
            };
        }

        public async Task AddNewTournamentAsync(Tournament tournament)
        {
            var user = await userRepository.GetByIdAsync(tournament.CreatorId) ??
                       throw new NotFoundInDatabaseException();

            var tournamentToAdd = await tournamentRepository.FindSingleAsync(x => x.Name == tournament.Name);

            if (tournamentToAdd != null)
            {
                throw new AlreadyInDatabaseException();
            }

            tournamentRepository.Add(tournament);
            await unitOfWork.CompleteAsync();
        }

        public async Task EditTournamentAsync(Tournament tournament)
        {
            var user = await userRepository.GetByIdAsync(tournament.CreatorId) ?? throw new NotFoundInDatabaseException();

            var tournamentToEdit = await tournamentRepository.FindSingleAsync(x => x.TournamentId == tournament.TournamentId) ?? throw new NotFoundInDatabaseException();

            tournamentRepository.ClearEntryState(tournamentToEdit);

            tournamentRepository.Update(tournament);
            await unitOfWork.CompleteAsync();
        }

        public async Task DeleteTournamentAsync(int id)
        {
            await teamService.DeleteTeamsRelatedToTheTournamentAsync(id);
            var tournamentToDelete = await tournamentRepository.GetByIdAsync(id) ?? throw new NotFoundInDatabaseException();

            tournamentRepository.Delete(tournamentToDelete);
            await unitOfWork.CompleteAsync();
        }

        public async Task<ICollection<Team>> GetTournamentTeamsAsync(int id)
        {
            var tournament = await tournamentRepository.GetByIdAsync(id) ?? throw new NotFoundInDatabaseException(); ;

            var teams = await teamRepository.FindAsync(team => team.TournamentId == id);

            return teams;
        }

        public async Task<ICollection<Player>> GetTournamentPlayersAsync(int id)
        {
            var teams = await teamRepository.FindAsync(t => t.TournamentId == id);

            var players =
                await playerRepository.FindAsync(p => teams.Select(t => t.TeamId).Contains(p.TeamId));

            if (players.Count == 0)
            {
                throw new NotFoundInDatabaseException();
            }

            return players;
        }

        public async Task<List<TeamWithPlayers>> GetTournamentPlayersGroupedByTeamAsync(int id)
        {
            var teams = await GetTournamentTeamsAsync(id);

            var listOfTeamsWithPlayers = new List<TeamWithPlayers>();

            foreach (var team in teams)
            {
                var players = await playerRepository.FindAsync(p => p.TeamId == team.TeamId);

                listOfTeamsWithPlayers.Add(new TeamWithPlayers()
                {
                    Team = team,
                    Players = players.ToArray()
                });
            }

            return listOfTeamsWithPlayers;
        }

        public async Task<ICollection<Match>> GetTournamentMatchesAsync(int id)
        {
            var matches =
                await matchRepository.FindAsync(m => m.HomeTeam.TournamentId == id && m.GuestTeam.TournamentId == id,new string[] {"HomeTeam","GuestTeam"});

            if (matches.Count == 0)
            {
                throw new NotFoundInDatabaseException();
            }

            return matches;
        }

        public async Task<List<DateWithMatches>> GetTournamentMatchesGroupedByDateAsync(int id)
        {
            var matches = await GetTournamentMatchesAsync(id);

            var listOfDateWithMatches = new List<DateWithMatches>();

            foreach (var match in matches.GroupBy(x => x.MatchDateTime.Date))
            {
                listOfDateWithMatches.Add(new DateWithMatches()
                {
                    DateTime = match.Key,
                    Matches = mapper.Map<List<MatchResponse>>(match.ToArray())
                });
            }

            return listOfDateWithMatches;
        }


        public async Task<TournamentPlayersPointsHolder> GetTournamentPoints(int tournamentId)
        {
            var tournament = await tournamentRepository.FindSingleAsync(t => t.TournamentId == tournamentId)
                             ?? throw new NotFoundInDatabaseException();

            var points = await pointsRepository.FindAsync(p => p.TournamentId == tournamentId, new string[] { "Tournament", "Player", "Match" });

            var teams = await teamRepository.FindAsync(t => t.TournamentId == tournamentId);

            var pointsGroupedSorted = points.GroupBy(p => p.Player)
                .Select(o => new TournamentPlayerPoints()
                {
                    PlayerId = o.Key.PlayerId,
                    Player = mapper.Map<PlayerResponse>(o.Key),
                    PointsQty = o.Sum(i => i.PointsQty)
                }).OrderByDescending(n => n.PointsQty)
                .ToList();

            return new TournamentPlayersPointsHolder(pointsGroupedSorted);
        }
    }
}