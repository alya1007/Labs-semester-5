using TMPS.Domain.Models.Abstractions;

namespace TMPS.Domain.Interfaces;
public interface IEmployeeRepository
{
    List<Employee> GetAllEmployees();
    Employee GetEmployeeById(string id);
    Employee AddEmployee(Employee employee);
    Employee UpdateEmployee(Employee employee);
    void DeleteEmployee(string id);
}
