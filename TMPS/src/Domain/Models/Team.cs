using TMPS.DataAccess.Employees;
using TMPS.Domain.Interfaces;
using TMPS.Domain.Models.Abstractions;
using TMPS.UseCases.Employees;

namespace TMPS.Domain.Models
{
    public class Team : ICompoundTeam
    {
        public string Name { get; set; }
        public List<Employee> Employees { get; set; } = new();

        public Team(string name)
        {
            Name = name;
        }

        public decimal CalculateSalary()
        {
            decimal totalSalary = 0;
            foreach (var employee in Employees)
            {
                totalSalary += SalaryCalculator.CalculateSalary(employee);
            }
            return totalSalary;
        }
    }
}
