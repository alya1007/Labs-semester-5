using TMPS.Domain.Models.Abstractions;

namespace TMPS.Domain.Interfaces;
public interface ISalaryCalculator
{
    decimal CalculateSalary(Employee employee);
}