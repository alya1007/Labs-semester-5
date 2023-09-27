using System.Text.Json.Serialization;
using TMPS.Domain.Models.Abstractions;

namespace TMPS.Domain.Models
{

    public class Manager : Employee
    {
        public int TeamSize { get; set; }

        public Manager(string? name, decimal baseSalary, int teamSize)
        {
            Id = System.Guid.NewGuid();
            Name = name;
            BaseSalary = baseSalary;
            BonusCoefficient = 0.15M;
            TeamSize = teamSize;
        }
    }
}