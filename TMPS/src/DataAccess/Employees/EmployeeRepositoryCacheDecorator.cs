using TMPS.Domain.Interfaces;
using TMPS.Domain.Models.Abstractions;

namespace TMPS.DataAccess.Employees;

public class EmployeeRepositoryCacheDecorator : EmployeeRepositoryDecorator
{
    private Dictionary<string, Employee> _cache = new();

    public EmployeeRepositoryCacheDecorator(IEmployeeRepository employeeRepository) : base(employeeRepository)
    {
    }

    public new List<Employee> GetAllEmployees()
    {
        if (_cache.Count == 0)
        {
            var employees = base.GetAllEmployees();
            foreach (var employee in employees)
            {
                _cache[employee.Id.ToString()] = employee;
            }
        }

        return _cache.Values.ToList();
    }

    public new Employee GetEmployeeById(string id)
    {
        if (_cache.ContainsKey(id))
        {
            return _cache[id];
        }

        var employee = base.GetEmployeeById(id);
        _cache[id] = employee;
        return employee;
    }

    public new Employee AddEmployee(Employee employee)
    {
        var addedEmployee = base.AddEmployee(employee);
        _cache[addedEmployee.Id.ToString()] = addedEmployee;
        return addedEmployee;
    }

    public new Employee UpdateEmployee(Employee employee)
    {
        var updatedEmployee = base.UpdateEmployee(employee);
        _cache[updatedEmployee.Id.ToString()] = updatedEmployee;
        return updatedEmployee;
    }

    public new void DeleteEmployee(string id)
    {
        base.DeleteEmployee(id);
        _cache.Remove(id);
    }
}