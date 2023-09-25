using TMPS.Domain.Models.Abstractions;

namespace TMPS.Domain.Interfaces;
public interface IEmployeeFactory
{
    public Employee CreateDeveloper(int id, string name, decimal baseSalary, List<string?> skills);
    public Employee CreateManager(int id, string name, decimal baseSalary, int teamSize);
    public Employee CreateHRManager(int id, string name, decimal baseSalary, string? department);
}