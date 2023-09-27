using System.Text.Json.Serialization;
using TMPS.Domain.Models.Abstractions;

namespace TMPS.Domain.Models
{

    public class HRManager : Employee
    {
        public string? Department { get; set; }

        public HRManager(string? name, decimal baseSalary, string? department)
        {
            Id = System.Guid.NewGuid();
            Name = name;
            BaseSalary = baseSalary;
            BonusCoefficient = 0.1M;
            Department = department;
        }
    }
}