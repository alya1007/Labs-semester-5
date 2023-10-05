using TMPS.Domain.Models;
using TMPS.Domain.Models.Abstractions;

namespace TMPS.Domain.Interfaces;
public interface IEmployeeFactory
{
    public Employee CreateDeveloper(string name, decimal baseSalary);
    public Employee CreateManager(string name, decimal baseSalary);
    public Employee CreateHRManager(string name, decimal baseSalary);
}