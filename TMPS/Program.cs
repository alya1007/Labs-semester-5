using TMPS.DataAccess.Employees;
using TMPS.Domain.Models.Abstractions;
using TMPS.Domain.Models;

namespace TMPS;

class Program
{
    static void Main(string[] args)
    {
        Employee employee1 = new Developer(1, "John", 1000, new List<string?>() { "C#", "Java" });
        EmployeeRepository employeeRepository = new EmployeeRepository();

        employeeRepository.AddEmployee(employee1);
        System.Console.WriteLine(employeeRepository.GetEmployeeById(1).Name);
    }
}