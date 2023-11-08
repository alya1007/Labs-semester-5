using TMPS.Application;
using TMPS.DataAccess.Departments;
using TMPS.DataAccess.Employees;
using TMPS.Domain.Factory;
using TMPS.Domain.Interfaces;

namespace TMPS.Client;
class Program
{
    static void Main(string[] args)
    {
        IEmployeeFactory employeeFactory = new EmployeeFactory();
        IEmployeeRepository employeeRepository = EmployeeRepository.GetInstance();
        EmployeeRepositoryDecorator cachedEmployeeRepository = new EmployeeRepositoryCacheDecorator(employeeRepository);
        IDepartmentRepository departmentRepository = new DepartmentRepository();
        Menu menu = new(employeeFactory, cachedEmployeeRepository);
        menu.Show();
    }
}