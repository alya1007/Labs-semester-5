using TMPS.Domain.Models.Abstractions;

namespace TMPS.Domain.Models
{
    public class Developer : Employee
    {
        public List<string?> Skills { get; set; }

        public Developer(int id, string? name, decimal baseSalary, List<string?> skills) : base(id, name, baseSalary)
        {
            BonusCoefficient = 0.2M;
            Skills = skills;
        }
    }
}