using TMPS.Domain.Interfaces;
using TMPS.UseCases.Employees;

namespace TMPS.Domain.Models.Abstractions
{
    public class Department : IDepartment
    {
        public string? Name { get; set; }
        public List<IDepartment> Teams = new();

        public Department(string name)
        {
            Name = name;
        }

        public void AddTeam(IDepartment team)
        {
            Teams.Add(team);
        }

        public void RemoveTeam(string teamName)
        {
            Teams.RemoveAll(team => team.Name == teamName);
        }

        public decimal CalculateSalary()
        {
            decimal totalSalary = 0;
            foreach (var team in Teams)
            {
                totalSalary += team.CalculateSalary();
            }
            return totalSalary;
        }
    }
}
