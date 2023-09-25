using TMPS.Domain.Models.Abstractions;

namespace TMPS.Domain.Models
{
    public class HRManager : Employee
    {
        public string? Department { get; set; }

        public HRManager(int id, string? name, decimal baseSalary, string? department) : base(id, name, baseSalary)
        {
            BonusCoefficient = 0.1M;
            Department = department;
        }
    }
}