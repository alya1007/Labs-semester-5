using System.Text.Json.Serialization;
using TMPS.DataAccess.Employees;
using TMPS.Domain.Interfaces;
using TMPS.Domain.Models.Abstractions;
using TMPS.UseCases.Employees;

namespace TMPS.Domain.Models
{
    public class Team : IDepartment
    {
        public string? Name { get; set; }
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
                var bonus = employee.BonusCoefficient * employee.BaseSalary;
                totalSalary += employee.BaseSalary + bonus;
            }
            return totalSalary;
        }
    }
}
