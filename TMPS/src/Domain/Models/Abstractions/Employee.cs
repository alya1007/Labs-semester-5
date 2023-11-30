using System.Text.Json.Serialization;
using TMPS.Domain.Interfaces;
using TMPS.Domain.Strategies;
using TMPS.UseCases.Employees;

namespace TMPS.Domain.Models.Abstractions
{
    [JsonDerivedType(typeof(Developer), typeDiscriminator: "developer")]
    [JsonDerivedType(typeof(Manager), typeDiscriminator: "manager")]
    [JsonDerivedType(typeof(HRManager), typeDiscriminator: "hrmanager")]
    public class Employee : IWorkUnit
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public decimal BaseSalary { get; set; }
        public decimal BonusCoefficient { get; set; }
        private ISalaryCalculator salaryCalculator = new DefaultSalaryCalculator();
        public ISalaryCalculator SalaryCalculator
        {
            get => salaryCalculator;
            set => salaryCalculator = value ?? throw new ArgumentNullException(nameof(value));
        }

        public virtual decimal CalculateSalary()
        {
            return salaryCalculator.CalculateSalary(this);
        }

    }
}