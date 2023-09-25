using TMPS.Domain.Interfaces;
using TMPS.Domain.Models;
using TMPS.Domain.Models.Abstractions;

namespace TMPS.Domain.Factory;

public class EmployeeFactory : IEmployeeFactory
{
    public Employee CreateDeveloper(int id, string name, decimal baseSalary, List<string?> skills)
    {
        return new Developer(id, name, baseSalary, skills);
    }

    public Employee CreateManager(int id, string name, decimal baseSalary, int teamSize)
    {
        return new Manager(id, name, baseSalary, teamSize);
    }

    public Employee CreateHRManager(int id, string name, decimal baseSalary, string? department)
    {
        return new HRManager(id, name, baseSalary, department);
    }

}