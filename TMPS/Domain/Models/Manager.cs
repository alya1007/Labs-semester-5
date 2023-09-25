using TMPS.Domain.Models.Abstractions;

namespace TMPS.Domain.Models
{
    public class Manager : Employee
    {
        public int TeamSize { get; set; }

        public Manager(int id, string? name, decimal baseSalary, int teamSize) : base(id, name, baseSalary)
        {
            BonusCoefficient = 0.15M;
            TeamSize = teamSize;
        }
    }
}