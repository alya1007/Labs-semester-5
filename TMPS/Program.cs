using TMPS.DataAccess.Employees;
using TMPS.Domain.Models;

namespace TMPS;

class Program
{
    static void Main(string[] args)
    {
        Employee employee1 = new Employee(1, "John", "Software Developer", 1000);
        EmployeeRepository employeeRepository = new EmployeeRepository();

        employeeRepository.AddEmployee(employee1);
        System.Console.WriteLine(employeeRepository.GetEmployeeById(1).Name);
    }
}