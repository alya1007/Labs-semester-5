using System.Text.Json.Serialization;
using TMPS.Domain.Models.Abstractions;

namespace TMPS.Domain.Models
{
    public class Developer : Employee
    {
        public List<string> Skills { get; set; }

        public Developer(string? name, decimal baseSalary, List<string> skills)
        {
            Id = Guid.NewGuid();
            Name = name;
            BaseSalary = baseSalary;
            BonusCoefficient = 0.2M;
            Skills = skills;
        }
    }
}