using TMPS.Domain.Models;
using TMPS.Domain.Models.Abstractions;

namespace TMPS.Domain.Interfaces
{
    public interface ITeamRepository
    {
        List<Team> GetTeams();
        Team GetTeamByName(string teamName);
        void AddTeam(Team team);
    }
}