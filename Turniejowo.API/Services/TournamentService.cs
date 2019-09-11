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
        private readonly IUnitOfWork unitOfWork;
        private readonly ITournamentRepository tournamentRepository;
        private readonly ITeamRepository teamRepository;
        private readonly IPlayerRepository playerRepository;
        private readonly IUserRepository userRepository;
        private readonly IMatchRepository matchRepository;
        private readonly IMatchToMatchResponseMapper matchToMatchResponseMapper;
        private readonly IMapper mapper;

        public TournamentService(IUnitOfWork unitOfWork, ITournamentRepository tournamentRepository, 
                                 ITeamRepository teamRepository, IPlayerRepository playerRepository, 
                                 IUserRepository userRepository, IMatchRepository matchRepository, IMatchToMatchResponseMapper matchToMatchResponseMapper, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.tournamentRepository = tournamentRepository;
            this.teamRepository = teamRepository;
            this.playerRepository = playerRepository;
            this.userRepository = userRepository;
            this.matchRepository = matchRepository;
            this.matchToMatchResponseMapper = matchToMatchResponseMapper;
            this.mapper = mapper;
        }

        public async Task<Tournament> GetTournamentByIdAsync(int id)
        {
            var tournament = await tournamentRepository.GetByIdAsync(id);

            if (tournament == null)
            {
                throw new NotFoundInDatabaseException();
            }

            return tournament;
        }

        public async Task AddNewTournamentAsync(Tournament tournament)
        {
            if (await userRepository.GetByIdAsync(tournament.CreatorId) == null)
            {
                throw new NotFoundInDatabaseException();
            }

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
            if (await userRepository.GetByIdAsync(tournament.CreatorId) == null)
            {
                throw new NotFoundInDatabaseException();
            }

            var tournamentToEdit = await tournamentRepository.FindSingleAsync(x => x.TournamentId == tournament.TournamentId);

            if (tournamentToEdit == null)
            {
                throw new NotFoundInDatabaseException();
            }

            tournamentRepository.ClearEntryState(tournamentToEdit);

            tournamentRepository.Update(tournament);
            await unitOfWork.CompleteAsync();
        }

        public async Task DeleteTournamentAsync(int id)
        {
            var tournamentToDelete = await tournamentRepository.GetByIdAsync(id);

            if (tournamentToDelete == null)
            {
                throw new NotFoundInDatabaseException();
            }

            tournamentRepository.Delete(tournamentToDelete);
            await unitOfWork.CompleteAsync();
        }

        public async Task<ICollection<Team>> GetTournamentTeamsAsync(int id)
        {
            var teams = await teamRepository.FindAsync(team => team.TournamentId == id);

            if (teams.Count == 0)
            {
                throw new NotFoundInDatabaseException();
            }

            return teams;
        }

        public async Task<ICollection<Player>> GetTournamentPlayersAsync(int id)
        {
            var teams = await teamRepository.FindAsync(t => t.TournamentId == id);

            if (teams.Count == 0)
            {
                throw new NotFoundInDatabaseException();
            }

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

            if (teams.Count == 0)
            {
                throw new NotFoundInDatabaseException();
            }

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
                await matchRepository.FindAsync(m => m.HomeTeam.TournamentId == id && m.GuestTeam.TournamentId == id);

            if (matches.Count == 0)
            {
                throw new NotFoundInDatabaseException();
            }

            return matches;
        }

        public async Task<List<DateWithMatches>> GetTournamentMatchesGroupedByDateAsync(int id)
        {
            var matches = await GetTournamentMatchesAsync(id);

            if (matches.Count == 0)
            {
                throw new NotFoundInDatabaseException();
            }

            var listOfDateWithMatches = new List<DateWithMatches>();

            foreach (var match in matches.GroupBy(x => x.MatchDateTime.Date))
            {
                listOfDateWithMatches.Add(new DateWithMatches()
                {
                    DateTime = match.Key,
                    Matches = await matchToMatchResponseMapper.Map(match.ToArray())
                });
            }

            return listOfDateWithMatches;
        }

        public async Task<Table> GetTournamentTable(int id)
        {
            var tournamentTeams = await GetTournamentTeamsAsync(id) ?? throw new NotFoundInDatabaseException();

            foreach (var match in await GetTournamentMatchesAsync(id))
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
    }
}