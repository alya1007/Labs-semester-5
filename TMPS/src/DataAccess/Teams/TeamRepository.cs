using TMPS.Domain.Interfaces;
using TMPS.Domain.Models;
using System.Text.Json;
using TMPS.Domain.Models.Abstractions;


namespace TMPS.DataAccess.Teams
{
    public class DepartmentRepository : ITeamRepository
    {
        private const string FilePath = "src/AppData/compound-teams.json";
        public List<Team> GetTeams()
        {
            try
            {
                if (!File.Exists(FilePath))
                {
                    return new List<Team>();
                }

                List<Team>? teams;
                using (StreamReader file = File.OpenText(FilePath))
                {
                    teams = JsonSerializer.Deserialize<List<Team>>(file.ReadToEnd());
                }

                return teams ?? new List<Team>();
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error parsing JSON: {ex.Message}");
                return new List<Team>();
            }
        }

        public Team GetTeamByName(string teamName)
        {
            List<Team> teams = GetTeams();
            return teams.FirstOrDefault(t => t.Name == teamName) ?? throw new ArgumentException("Team not found.");
        }

        public void AddTeam(Team team)
        {
            List<Team> teams = GetTeams();
            teams.Add(team);
            SaveTeamsToJson(teams);
        }

        private void SaveTeamsToJson(List<Team> teams)
        {
            try
            {
                string jsonString = JsonSerializer.Serialize(teams);
                File.WriteAllText(FilePath, jsonString);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error parsing JSON: {ex.Message}");
            }
        }
    }
}