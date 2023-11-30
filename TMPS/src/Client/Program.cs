using System.Collections;
using TMPS.Application;
using TMPS.DataAccess.Departments;
using TMPS.DataAccess.Employees;
using TMPS.Domain.Factory;
using TMPS.Domain.Interfaces;

class Program
{
    public static void Main(string[] args)
    {

        IEmployeeFactory employeeFactory = new EmployeeFactory();
        IEmployeeRepository employeeRepository = EmployeeRepository.GetInstance();
        IDepartmentRepository departmentRepository = DepartmentRepository.GetInstance();

        EmployeeRepositoryDecorator cachedEmployeeRepository = new EmployeeRepositoryCacheDecorator(employeeRepository);

        Menu menu = new(employeeFactory, cachedEmployeeRepository, departmentRepository);
        menu.Show();
    }
}