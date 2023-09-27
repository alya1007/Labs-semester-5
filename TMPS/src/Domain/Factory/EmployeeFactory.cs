using TMPS.Domain.Interfaces;
using TMPS.Domain.Models;
using TMPS.Domain.Models.Abstractions;

namespace TMPS.Domain.Factory;

public class EmployeeFactory : IEmployeeFactory
{
    public Developer CreateDeveloper(string name, decimal baseSalary)
    {
        return new Developer(name, baseSalary, new List<string>());
    }

    public Manager CreateManager(string name, decimal baseSalary)
    {
        return new Manager(name, baseSalary, 0);
    }

    public HRManager CreateHRManager(string name, decimal baseSalary)
    {
        return new HRManager(name, baseSalary, "");
    }

}