using System;
using System.Collections.Generic;
using TMPS.DataAccess.Employees;
using TMPS.Domain.Interfaces;
using TMPS.Domain.Models;

namespace TMPS.Services
{
    public class EmployeeService
    {
        private readonly IEmployeeManagement employeeManagement;

        public EmployeeService(IEmployeeManagement employeeManagement)
        {
            this.employeeManagement = employeeManagement;
        }

        public void AddEmployee(Employee employee)
        {
            Employee? newEmployee = null;
            // Create an instance of the appropriate employee type based on the input
            switch (employee?.Position?.ToLower())
            {
                case "developer":
                    newEmployee = new Developer(employee.Name, employee.Position, employee.BaseSalary);
                    break;
                case "manager":
                    newEmployee = new Manager { Name = name };
                    break;
                case "hrmanager":
                    newEmployee = new HRManager { Name = name };
                    break;
                default:
                    throw new ArgumentException("Invalid new employee type.");
            }

            // Add the employee to the repository
            employeeManagement.AddEmployee(employee);
        }

        public List<Employee> GetAllEmployees()
        {
            return employeeManagement.GetAllEmployees();
        }
    }
}
