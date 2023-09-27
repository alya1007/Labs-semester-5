using TMPS.Domain.Models.Abstractions;

namespace TMPS.UseCases.Employees;

public class SalaryCalculator
{
    public static decimal CalculateSalary(Employee employee)
    {
        decimal bonus = employee.BonusCoefficient * employee.BaseSalary;
        return employee.BaseSalary + bonus;
    }
}