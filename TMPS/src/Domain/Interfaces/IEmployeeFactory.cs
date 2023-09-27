using TMPS.Domain.Models;
using TMPS.Domain.Models.Abstractions;

namespace TMPS.Domain.Interfaces;
public interface IEmployeeFactory
{
    public Developer CreateDeveloper(string name, decimal baseSalary);
    public Manager CreateManager(string name, decimal baseSalary);
    public HRManager CreateHRManager(string name, decimal baseSalary);
}