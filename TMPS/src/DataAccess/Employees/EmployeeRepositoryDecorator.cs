using TMPS.Domain.Interfaces;
using TMPS.Domain.Models.Abstractions;

namespace TMPS.DataAccess.Employees
{
    public class EmployeeRepositoryDecorator : IEmployeeRepository
    {
        private IEmployeeRepository _employeeRepository;

        public EmployeeRepositoryDecorator(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public List<Employee> GetAllEmployees()
        {
            return _employeeRepository.GetAllEmployees();
        }

        public Employee GetEmployeeById(string id)
        {
            return _employeeRepository.GetEmployeeById(id);
        }

        public Employee AddEmployee(Employee employee)
        {
            return _employeeRepository.AddEmployee(employee);
        }

        public Employee UpdateEmployee(Employee employee)
        {
            return _employeeRepository.UpdateEmployee(employee);
        }

        public void DeleteEmployee(string id)
        {
            _employeeRepository.DeleteEmployee(id);
        }

    }
}