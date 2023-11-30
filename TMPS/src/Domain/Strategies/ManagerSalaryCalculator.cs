using TMPS.Domain.Interfaces;
using TMPS.Domain.Models;
using TMPS.Domain.Models.Abstractions;

namespace TMPS.Domain.Strategies;
public class ManagerSalaryCalculator : ISalaryCalculator
{
    public decimal CalculateSalary(Employee employee)
    {
        if (employee is Manager manager && manager.TeamSize > 10) return employee.BaseSalary + employee.BonusCoefficient * employee.BaseSalary + 1000;
        else return employee.BaseSalary + employee.BonusCoefficient * employee.BaseSalary;
    }
}
