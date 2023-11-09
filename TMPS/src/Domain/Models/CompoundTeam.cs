using TMPS.Domain.Interfaces;
using TMPS.UseCases.Employees;

namespace TMPS.Domain.Models.Abstractions
{
    public class CompoundTeam : ICompoundTeam
    {
        public string? Name { get; set; }
        private List<ICompoundTeam> teams = new();

        public void AddTeam(ICompoundTeam team)
        {
            teams.Add(team);
        }

        public void RemoveTeam(ICompoundTeam team)
        {
            teams.Remove(team);
        }

        public decimal CalculateSalary()
        {
            decimal totalSalary = 0;
            foreach (var team in teams)
            {
                totalSalary += team.CalculateSalary();
            }
            return totalSalary;
        }
    }
}
