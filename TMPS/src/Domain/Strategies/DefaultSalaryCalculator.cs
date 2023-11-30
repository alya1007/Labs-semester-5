using TMPS.Domain.Interfaces;
using TMPS.Domain.Models.Abstractions;

namespace TMPS.Domain.Strategies;

public class DefaultSalaryCalculator : ISalaryCalculator
{
    public decimal CalculateSalary(Employee employee)
    {
        return employee.BaseSalary + employee.BonusCoefficient * employee.BaseSalary;
    }
}

